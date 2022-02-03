import { Dispatch, SetStateAction, useEffect, useState } from "react";

interface ErrorMessageProps {
    className?: string,
    error: string,
    setError: Dispatch<SetStateAction<string>>
}

export default function ErrorMessage(props: ErrorMessageProps) {
    const [fadeOut, setFadeOut] = useState<boolean>(false);

    const handleTimer = () => {
        return setTimeout(() => {
            setFadeOut(true);
        }, 5000);
    };

    const onAnimationEnd = () => {
        if (fadeOut) {
            props.setError(null);
        }
    };

    useEffect(() => {
        const timeoutId = handleTimer();

        return () => {
            clearTimeout(timeoutId);
        };
    }, []);

    return (
        <div
            className={`
                flex
                justify-center
                bg-[#fb5c8e]/20
                px-[25px]
                py-[15px]
                rounded-[25px]
                ${ !fadeOut && 'animate-fade-in-down' }
                ${ fadeOut && 'animate-fade-out-up' }
            `}
            onAnimationEnd={onAnimationEnd}
        >
            <p className="font-semibold text-[#fb5c8e]">{props.error}</p>
        </div>
    );
}
