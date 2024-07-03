import type { NextPage } from 'next'
import { ReceivedMessageLog } from '../components/ReceivedMessageLog'
import { ReceivedMessage } from '../components/ReceivedMessageLogItem'
import { SignalRService } from '../generated/TypedSignalR.Client.DevTools'
import { useCallback, useEffect, useState } from 'react'
import { SignalRServiceConsoleContainer } from '../components/SignalRServiceConsoleContainer'

const Home: NextPage = () => {

    const [messages, setMessages] = useState<ReceivedMessage[]>([])

    const [services, setServices] = useState<SignalRService[]>([]);

    const setMessagesCallBack = useCallback((x: ReceivedMessage) => setMessages(prev => [...prev, x]), [])

    useEffect(() => {
        const f = async () => {
            const data = await fetch("./spec.json") // spec.json is generated by TypedSignalR.Client.DevTools.Specification
            const services = await data.json() as SignalRService[];
            console.log(services);
            setServices(services);
        }

        f();
    }, [])


    return (
        <div className="columns h-screen w-screen">
            <div className="column is-two-thirds scrollarea">
                <SignalRServiceConsoleContainer services={services} setReceivedMessage={setMessagesCallBack} />
            </div>
            <div className="column is-one-thirds">
                <div>
                    <ReceivedMessageLog messages={messages}></ReceivedMessageLog>
                </div>
            </div>
        </div>
    )
}

export default Home
