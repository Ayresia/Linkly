interface ButtonProps {
    className?: string,
    children: string
}

export default function Button(props: ButtonProps) {
    return (
        <button 
            className={`bg-[#0f53fa] px-[35px] py-[15px] font-bold focus:outline-none button-shadow text-white ${props.className}`}
        >
            {props.children}
        </button>
    );
}
