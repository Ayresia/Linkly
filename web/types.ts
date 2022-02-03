import { ChangeEventHandler, Dispatch, MouseEventHandler, SetStateAction } from "react";

export interface UrlInterface {
    url: string
}

export interface ShortenUrlResponse {
    slug: string
}

export interface ErrorResponse {
    status: number,
    error: string
}

export interface ButtonProps {
    className?: string,
    children: string,
    disabled: boolean,
    onClick?: MouseEventHandler<Element>
}

export interface ErrorMessageProps {
    className?: string,
    error: string,
    setError: Dispatch<SetStateAction<string>>
}

export interface InputBoxProps {
    className?: string,
    placeholder: string,
    disabled: boolean,
    onChange: ChangeEventHandler<HTMLInputElement>
}

export interface LinkPreviewProps {
    className?: string,
    link: string
}
