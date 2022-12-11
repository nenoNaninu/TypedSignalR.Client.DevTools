import { SignalRServiceConsole } from "./SignalRServiceConsole";
import { FC } from "react";
import { SignalRService } from "../generated/TypedSignalR.Client.DevTools";
import { ReceivedMessage } from './ReceivedMessageLogItem'

type Props = {
    services: SignalRService[]
    setReceivedMessage: (message: ReceivedMessage) => void;
}

export const SignalRServiceConsoleContainer: FC<Props> = props => {

    const serviceConsoles = props
        .services
        .map((x, index) =>
            <SignalRServiceConsole
                key={index}
                service={x}
                index={index}
                setReceivedMessage={props.setReceivedMessage} />)

    return (
        <div className="m-5">
            {serviceConsoles}
        </div>
    )
}
