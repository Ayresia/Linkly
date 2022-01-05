import { ChangeEventHandler } from "react";

interface InputBoxProps {
    className?: string,
    placeholder: string,
    onChange: ChangeEventHandler<HTMLInputElement>
}

export default function InputBox(props: InputBoxProps) {
    return (
        <input 
            onChange={props.onChange}
            className={`bg-[#292929] px-[25px] py-[15px] input-color focus:outline-none ${props.className}`} 
            placeholder={props.placeholder}
        />
    );
}
