import { ButtonProps } from "@types";
import { Loading } from "./loading";

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
