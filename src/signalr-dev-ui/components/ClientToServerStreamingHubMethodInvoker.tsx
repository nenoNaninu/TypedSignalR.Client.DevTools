import { FC, useState, useEffect, useCallback, Dispatch, SetStateAction } from 'react'
import { MethodMetadata } from '../generated/TypedSignalR.Client.DevTools'
import { HubConnection, Subject } from '@microsoft/signalr'
import { getInputTextParameterTransformer } from './InputTextParameterTransformer'
import { SimpleCodePresenter } from './SimpleCodePresenter'

type Props = {
    method: MethodMetadata,
    hubConnection: HubConnection | null
}

export const ClientToServerStreamingHubMethodInvoker: FC<Props> = props => {

    const method = props.method;
    const hubConnection = props.hubConnection;

    const [args, setArgs] = useState<any[]>([]);
    const [subject, setSubject] = useState<Subject<any> | null>(null);
    const [message, setMessage] = useState<string | null>(null);

    useEffect(() => {
        const array = new Array(props.method.parameters.length);
        setArgs(array);
    }, [props.method])

    const invoke = useCallback(() => {
        const f = async () => {
            console.log(args)

            const newSubject = new Subject<any>();
            setSubject(newSubject);

            try {
                const newArgs = InsertSubjectToArgument(method, args, newSubject);

                await hubConnection?.send(method.methodName, ...newArgs);

                setMessage("Stream started.");
            } catch (e) {
                setMessage(`${e}`);
                setSubject(null);
                newSubject?.error(e);
            }
        }

        f();
    }, [args, method, hubConnection])

    const streamIndex = FindStreamParameterIndex(method);

    const parameterInputFields = props.method
        .parameters
        .map((x, index) => (
            <ParameterInputField
                key={index}
                name={x.name}
                typeName={x.typeName}
                index={index}
                setArgs={setArgs} />
        ));

    return (
        <div className='box'>
            <label className="label is-large">{method.methodName}</label>
            {parameterInputFields}
            <button className="button is-info" disabled={(hubConnection ? false : true) || (subject ? true : false)} onClick={_ => invoke()}>Invoke</button>
            <NextParameterInputField
                typeName={method.parameters[streamIndex].typeName}
                name={method.parameters[streamIndex].name}
                subject={subject} />
            <button className="button is-info ml-3"
                disabled={subject ? false : true}
                onClick={_ => {
                    subject?.complete();
                    setSubject(null);
                    setMessage("Stream stopped.");

                }}>
                Complete
            </button>
            <button className="button is-info ml-3"
                disabled={subject ? false : true}
                onClick={_ => {
                    subject?.error("The stream has been canceled.");
                    setSubject(null);
                    setMessage("Stream stopped.");
                }}>
                Cancel
            </button>
            {message && (
                <>
                    <h5 className='m-3'>response : {method.returnType}</h5>
                    <SimpleCodePresenter text={message} />
                </>
            )}
        </div >
    )
}

type ParameterInputFieldProps = {
    name: string,
    typeName: string,
    index: number,
    setArgs: Dispatch<SetStateAction<any[]>>,
}

const ParameterInputField: FC<ParameterInputFieldProps> = props => {

    const [intputException, setIntputException] = useState<any>(null)

    const { name, typeName, index, setArgs } = props;

    return (
        <div className='field'>
            <label className="label">{name} : {typeName}</label>
            <div className="control">
                <textarea
                    className={intputException ? 'input is-danger' : 'input'}
                    placeholder={typeName}
                    disabled={IsStreamType(typeName)}
                    onBlur={e => {
                        try {
                            const text = e.target.value;

                            const transformer = getInputTextParameterTransformer(typeName);
                            const instance = transformer(text);
                            setArgs(prev => {
                                prev[index] = instance;
                                return [...prev];
                            });

                            setIntputException(undefined);
                        }
                        catch (ex) {
                            setArgs(prev => {
                                prev[index] = null;
                                return [...prev];
                            });

                            setIntputException(`${ex}`);
                        }
                    }} />
            </div>
            {intputException && <p className="help is-danger">{`${intputException}`}</p>}
        </div>
    );
}


const asyncEnumerable = "global::System.Collections.Generic.IAsyncEnumerable"
const channelReader = "global::System.Threading.Channels.ChannelReader"

const IsStreamType = (typeName: string) => {
    if (typeName.startsWith(asyncEnumerable)) {
        return true;
    }

    if (typeName.startsWith(channelReader)) {
        return true;
    }

    return false;
}

const FindStreamParameterIndex = (method: MethodMetadata) => {
    for (let i = 0; i < method.parameters.length; i++) {
        if (IsStreamType(method.parameters[i].typeName)) {
            return i;
        }
    }

    return -1;
}

const InsertSubjectToArgument = (method: MethodMetadata, args: any[], subject: Subject<any>) => {

    const index = FindStreamParameterIndex(method);

    if (index < 0) {
        return args;
    }

    args[index] = subject;
    return args;
}

type NextParameterInputFieldProps = {
    name: string,
    typeName: string,
    subject: Subject<any> | null,
}

const NextParameterInputField: FC<NextParameterInputFieldProps> = props => {

    const [message, setMessage] = useState<any>(null);
    const [intputException, setIntputException] = useState<any>(null)

    const { name, typeName, subject } = props;

    return (
        <>
            <div className='field'>
                <label className="label">{name} : {typeName}</label>
                <div className="control">
                    <textarea
                        className={intputException ? 'input is-danger' : 'input'}
                        placeholder={typeName}
                        disabled={subject ? false : true}
                        onBlur={e => {
                            try {
                                const text = e.target.value;

                                const transformer = getInputTextParameterTransformer(getGenericTypeArgument(typeName));
                                const instance = transformer(text);

                                setMessage(instance);
                                setIntputException(undefined);
                            }
                            catch (ex) {
                                setMessage(null);
                                setIntputException(`${ex}`);
                            }
                        }} />
                </div>
                {intputException && <p className="help is-danger">{`${intputException}`}</p>}
            </div>
            <button className="button is-info" disabled={subject ? false : true} onClick={_ => subject?.next(message)}>Next</button>
        </>
    );
}

const getGenericTypeArgument = (typeName: string) => {

    if (typeName.startsWith(asyncEnumerable)) {
        const start = asyncEnumerable.length + 1; // <
        const end = typeName.length - 1; // >

        return typeName.substring(start, end)
    }

    if (typeName.startsWith(channelReader)) {
        const start = channelReader.length + 1; // <
        const end = typeName.length - 1; // >

        return typeName.substring(start, end)
    }

    return ""; // unreachable 
}
