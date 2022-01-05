interface InputBoxProps {
    className?: string
    placeholder: string
}

export default function InputBox(props: InputBoxProps) {
    return (
        <input 
            className={`bg-[#292929] px-[25px] py-[15px] input-color focus:outline-none ${props.className}`} 
            placeholder={props.placeholder}
        />
    );
}
