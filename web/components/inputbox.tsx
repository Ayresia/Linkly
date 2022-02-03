import { InputBoxProps } from "@types";

export default function InputBox(props: InputBoxProps) {
    return (
        <input 
            disabled={props.disabled}
            onChange={props.onChange}
            className={`bg-[#292929] px-[25px] py-[15px] input-color focus:outline-none ${props.className}`} 
            placeholder={props.placeholder}
        />
    );
}
