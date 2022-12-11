import { HubConnection } from "@microsoft/signalr"
import { FC } from "react"
import { MethodMetadata } from "../generated/TypedSignalR.Client.DevTools"
import { HubMethodInvoker } from "./HubMethodInvoker"
import { ServerToClientStreamingHubMethodInvoker } from "./ServerToClientStreamingHubMethodInvoker"
import { ReceivedMessage } from "./ReceivedMessageLogItem"
import { ClientToServerStreamingHubMethodInvoker } from "./ClientToServerStreamingHubMethodInvoker"


type Props = {
    method: MethodMetadata,
    hubConnection: HubConnection | null,
    setReceivedMessage: (message: ReceivedMessage) => void;
}


export const HubMethodInvokerSwitcher: FC<Props> = props => {

    const { method, hubConnection, setReceivedMessage } = props;

    // server-to-client streaming
    if (isServerToClientStreamingHubMethod(method)) {
        return (
            <ServerToClientStreamingHubMethodInvoker method={method} hubConnection={hubConnection} setStreamMessage={setReceivedMessage} />
        )
    }
    // server-to-client streaming
    else if (isClientToServerStreamingHubMethod(method)) {
        return (
            <ClientToServerStreamingHubMethodInvoker method={method} hubConnection={hubConnection} />
        )
    }
    // ordinary invocation
    else {
        return (
            <HubMethodInvoker method={method} hubConnection={hubConnection} />
        )
    }
}

const isServerToClientStreamingHubMethod = (method: MethodMetadata) => {
    if (method.returnType.startsWith("global::System.Collections.Generic.IAsyncEnumerable")) {
        return true;
    }

    if (method.returnType.startsWith("global::System.Threading.Tasks.Task<global::System.Collections.Generic.IAsyncEnumerable")) {
        return true;
    }

    if (method.returnType.startsWith("global::System.Threading.Tasks.Task<global::System.Threading.Channels.ChannelReader")) {
        return true;
    }

    return false;
}

const isClientToServerStreamingHubMethod = (method: MethodMetadata) => {

    for (const parameter of method.parameters) {
        if (parameter.typeName.startsWith("global::System.Collections.Generic.IAsyncEnumerable")) {
            return true;
        }

        if (parameter.typeName.startsWith("global::System.Threading.Channels.ChannelReader")) {
            return true;
        }
    }

    return false;
}
