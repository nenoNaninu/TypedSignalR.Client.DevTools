import { FC } from 'react'

export type Props = {
    text: string | null
}

export const SimpleCodePresenter: FC<Props> = props => {
    return (
        <>
            {
                props.text && (
                    <pre className='has-background-dark has-text-white pre-code'>
                        <code className='wrap-anywhere'>{props.text}</code>
                    </pre>
                )
            }
        </>
    )
}
