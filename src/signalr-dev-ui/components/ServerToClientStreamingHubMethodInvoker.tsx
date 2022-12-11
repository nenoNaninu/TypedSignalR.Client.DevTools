import { FC, useState, useEffect, useCallback, Dispatch, SetStateAction } from 'react'
import { MethodMetadata } from '../generated/TypedSignalR.Client.DevTools'
import { HubConnection } from '@microsoft/signalr'
import { getInputTextParameterTransformer } from './InputTextParameterTransformer'
import { SimpleCodePresenter } from './SimpleCodePresenter'
import { ReceivedMessage } from './ReceivedMessageLogItem'

type Props = {
    method: MethodMetadata,
    hubConnection: HubConnection | null,
    setStreamMessage: (message: ReceivedMessage) => void;
}

export const ServerToClientStreamingHubMethodInvoker: FC<Props> = props => {

    const method = props.method;
    const hubConnection = props.hubConnection;
    const setStreamMessage = props.setStreamMessage;

    const [args, setArgs] = useState<any[]>([]);
    const [message, setMessage] = useState<string | null>(null);

    useEffect(() => {
        const array = new Array(props.method.parameters.length);
        setArgs(array);
    }, [props.method])

    const invoke = useCallback(() => {
        const f = async () => {
            console.log("=== ServerToClientStreamingHubMethodInvoker ===")
            console.log(args)
            const filteredArgs = RemoveCancellationTokenParameter(method, args);
            console.log(filteredArgs);

            try {
                hubConnection?.stream<any>(method.methodName, ...filteredArgs).subscribe({
                    next: (item) => {
                        setStreamMessage({ methodName: `${method.methodName} : OnNext`, content: item });
                    },
                    complete: () => {
                        setStreamMessage({ methodName: `${method.methodName} : OnCompleted`, content: "" });
                        setMessage("Stream stoped.");
                    },
                    error: (e) => {
                        setStreamMessage({ methodName: `${method.methodName} : OnError`, content: `${e}` });
                        setMessage("Stream stoped.");
                    },
                });

                setMessage("Stream started.");
            } catch (e) {
                setMessage(`${e}`);
            }
        }

        f();
    }, [args, method, hubConnection, setMessage, setStreamMessage])

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
            <button className="button is-info" disabled={hubConnection ? false : true} onClick={_ => invoke()}>Invoke</button>
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
                    disabled={typeName === "global::System.Threading.CancellationToken"}
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

const RemoveCancellationTokenParameter = (method: MethodMetadata, args: any[]) => {

    const index = method.parameters
        .map(x => x.typeName)
        .indexOf("global::System.Threading.CancellationToken");

    if (index < 0) {
        return args;
    }

    const array = [...args];
    array.splice(index, 1);
    return array;
}
