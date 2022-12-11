import React, { FC, useState, useEffect } from 'react';
import { SimpleCodePresenter } from './SimpleCodePresenter';

export type ReceivedMessage = {
    methodName: string,
    /**
     * Muse be POCO.
     * Content is serialized to JSON and displayed.
     */
    content: any,
}

export type Options = {
    indent: boolean
}

export type Props = {
    message: ReceivedMessage,
    options: Options
}

export const ReceivedMessageLogItem: FC<Props> = props => {

    const [displayContent, setDisplayContent] = useState("");

    const options = props.options;
    const message = props.message;

    useEffect(() => {
        const jsonStr = JSON.stringify(message.content, null, 4)

        setDisplayContent(jsonStr);
    }, [message, options])

    return (
        <div className="box m-2">
            <div className="is-large">
                <strong>{props.message.methodName}</strong>
            </div>
            <div className='m-3'>
                {options.indent ? (
                    <SimpleCodePresenter text={displayContent} />
                ) : (
                    <div className='wrap-anywhere'>
                        {displayContent}
                    </div>
                )}
            </div>
        </div>
    )
}
