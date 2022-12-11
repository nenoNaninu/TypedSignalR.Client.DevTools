import { ReceivedMessage, ReceivedMessageLogItem } from "./ReceivedMessageLogItem";
import { FC, useState } from 'react'

export type Props = {
    messages: ReceivedMessage[]
}

export const ReceivedMessageLog: FC<Props> = props => {

    const [indentOption, setIndentOption] = useState(false);

    const items = props
        .messages
        .map((x, index) => (
            <div key={index} >
                <ReceivedMessageLogItem message={{ methodName: x.methodName, content: x.content }} options={{ indent: indentOption }} />
            </div>))
        .reverse();

    return (
        <div className="h-screen vertical-container">
            <div className="box">
                <div className="level">
                    <div className="level-left">
                        <div className="is-large">
                            <strong>Receive Message Log</strong>
                        </div>
                    </div>
                    <div className="level-right">
                        <div className="content">
                            <label className="checkbox">
                                <input type="checkbox" onChange={x => {
                                    setIndentOption(x.target.checked)
                                }} />
                                indent
                            </label>
                        </div>
                    </div>
                </div>
            </div>
            <div className="h-max w-max">
                <div className="has-background-light scrollarea h-max">
                    {items}
                </div>
            </div>

        </div>
    )
}
