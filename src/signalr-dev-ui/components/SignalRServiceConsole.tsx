import { FC, useState, useCallback, Dispatch, SetStateAction } from "react"
import { MethodMetadata, SignalRService } from "../generated/TypedSignalR.Client.DevTools"
import { HubConnection, HubConnectionBuilder, LogLevel, IHttpConnectionOptions } from '@microsoft/signalr'
import { SimpleCodePresenter } from "./SimpleCodePresenter"
import { HubMethodInvokerSwitcher } from "./HubMethodInvokerSwitcher"
import { ReceivedMessage } from "./ReceivedMessageLogItem"

const backGroundColors = [
    "has-background-info-light",
    "has-background-primary-light",
    "has-background-danger-light"
]

type Props = {
    service: SignalRService,
    index: number,
    setReceivedMessage: (message: ReceivedMessage) => void;
}

export const SignalRServiceConsole: FC<Props> = props => {

    const service = props.service;
    const setReceivedMessage = props.setReceivedMessage;
    const authenticationSchemes = service.authenticationSchemes ?? [];

    const [hubConnection, setHubConnection] = useState<HubConnection | null>(null);
    const [connectionStatusMessage, setConnectionStatusMessage] = useState<string | null>(null);
    const [jwt, setJwt] = useState<string | null>(null);

    // Check for JWT or Windows authentication
    const hasJwtAuth = authenticationSchemes.some(s => s.toLowerCase().includes("bearer") || s.toLowerCase().includes("jwt"));
    const hasWindowsAuth = authenticationSchemes.some(s => s.toLowerCase().includes("negotiate") || s.toLowerCase().includes("windows"));
    const shouldShowAuthSchemes = service.isAuthRequired && authenticationSchemes.length > 0;

    const connectToHub = useCallback(() => {
        const f = async () => {

            if (hubConnection) {
                await hubConnection.stop();
            }

            // JWT authentication
            if (hasJwtAuth && !jwt) {
                setConnectionStatusMessage("Please input JWT.")
                return;
            }

            const connectionOptions: IHttpConnectionOptions = {};
            if (hasJwtAuth) connectionOptions.accessTokenFactory = () => jwt!;
            if (hasWindowsAuth) connectionOptions.withCredentials = true;

            const connection = new HubConnectionBuilder()
                .withUrl(service.path, connectionOptions)
                .withAutomaticReconnect()
                .configureLogging(LogLevel.Information)
                .build();

            for (const method of service.receiverType.methods) {
                connection.on(method.methodName, (...args) => setReceivedMessage({ methodName: method.methodName, content: args }))
            }

            try {
                await connection.start();
                setHubConnection(connection);
                setConnectionStatusMessage("Connection succeeded.")
            }
            catch (e: any) {
                setConnectionStatusMessage(`Exception occurred:\n ${e}`)
            }
        }

        f();
    }, [hubConnection, jwt, service, setHubConnection, setConnectionStatusMessage, setReceivedMessage, hasJwtAuth, hasWindowsAuth])

    return (
        <div className={`container is-max-widescreen ${backGroundColors[props.index % 3]} my-6`}>
            <div className="content p-5">
                <SignalRServiceMetadata service={service} />
                {hasJwtAuth && (
                    <JwtInputField isAuthRequired={true} hubConnection={hubConnection} setJwt={setJwt} />
                )}
                {shouldShowAuthSchemes && (
                    <div>
                        <strong>Supported Authentication Schemes:</strong>
                        <ul>
                            {authenticationSchemes.map((scheme, idx) => (
                                <li key={idx}>{scheme}</li>
                            ))}
                        </ul>
                    </div>
                )}
                <button className="button is-warning my-5" onClick={() => connectToHub()} >Connect to Hub</button>
                <SimpleCodePresenter text={connectionStatusMessage} />
                <HubInvokerList methods={service.hubType.methods} hubConnection={hubConnection} setReceivedMessage={setReceivedMessage} ></HubInvokerList>
            </div >
        </div >
    )
}

type SignalRServiceMetadataProps = {
    service: SignalRService,
}

const SignalRServiceMetadata: FC<SignalRServiceMetadataProps> = props => {

    const service = props.service;

    return (
        <>
            <h1 className="is-size-2">{service.name}</h1>
            <ul>
                <li>Hub Type: <strong>{service.hubType.interfaceName}</strong></li>
                <li>Receiver Type: <strong>{service.receiverType.interfaceName}</strong></li>
                <li>Path: <strong>{service.path}</strong></li>
                {service.isAuthRequired && (
                    <li><strong>Authorization Required</strong></li>
                )}
            </ul>
        </>
    )
}

type JwtInputFieldProps = {
    isAuthRequired: boolean,
    hubConnection: HubConnection | null,
    setJwt: Dispatch<SetStateAction<string | null>>
}

const JwtInputField: FC<JwtInputFieldProps> = props => {

    const { isAuthRequired, hubConnection, setJwt } = props;

    return (
        <>
            {isAuthRequired && (
                <div className="field">
                    <p className="control">
                        <input
                            className="input"
                            type="password"
                            placeholder="Input JWT"
                            onBlur={e => {
                                const text = e.target.value;
                                setJwt(text);
                            }}
                            disabled={hubConnection ? true : false} />
                    </p>
                </div>
            )}
        </>
    )
}

type HubInvokerListProps = {
    methods: MethodMetadata[],
    hubConnection: HubConnection | null,
    setReceivedMessage: (message: ReceivedMessage) => void;
}

const HubInvokerList: FC<HubInvokerListProps> = props => {

    const { methods, hubConnection, setReceivedMessage } = props;

    const hubInvokers = methods.map((x, index) => (
        <HubMethodInvokerSwitcher
            key={index}
            method={x}
            hubConnection={hubConnection}
            setReceivedMessage={setReceivedMessage} />
    ));

    return (
        <>
            {hubInvokers}
        </>
    )
}
