import { MouseEventHandler } from "react";
import { Loading } from "./loading";

interface ButtonProps {
    className?: string,
    children: string,
    disabled: boolean,
    onClick?: MouseEventHandler<Element>
}

export default function Button(props: ButtonProps) {
    return (
        <button 
            onClick={props.onClick}
            disabled={props.disabled}
            className=
                {`flex
                flex-row
                justify-center
                items-center
                bg-[#0f53fa]
                disabled:bg-[#103ba7]
                px-[35px] py-[15px]
                font-bold
                focus:outline-none
                button-shadow
                text-white
                ${props.className}`}
        >
            { props.disabled && <Loading /> }
            {props.children}
        </button>
    );
}
