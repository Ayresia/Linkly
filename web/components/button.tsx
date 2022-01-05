import { MouseEventHandler } from "react";

interface ButtonProps {
    className?: string,
    children: string,
    onClick?: MouseEventHandler<Element>
}

export default function Button(props: ButtonProps) {
    return (
        <button 
            onClick={props.onClick}
            className={`bg-[#0f53fa] px-[35px] py-[15px] font-bold focus:outline-none button-shadow text-white ${props.className}`}
        >
            {props.children}
        </button>
    );
}
