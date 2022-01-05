import { Dispatch, SetStateAction } from "react";

interface ErrorMessageProps {
    className?: string,
    error: string,
    setError: Dispatch<SetStateAction<string>>
}

export default function ErrorMessage(props: ErrorMessageProps) {
    setTimeout(() => {
        props.setError(null)
    }, 5000);

    return (
        <div
            className={`
                flex
                justify-center
                bg-[#fb5c8e]/20
                px-[25px]
                py-[15px]
                rounded-[25px]
                ${props.className}
            `} 
        >
            <p className="font-semibold text-[#fb5c8e]">{props.error}</p>
        </div>
    );
}
