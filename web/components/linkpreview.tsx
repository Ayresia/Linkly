import Image from 'next/image';

interface LinkPreviewProps {
    className?: string,
    link: string
}

export default function LinkPreview(props: LinkPreviewProps) {
    return (
        <div
            className={`
                flex
                justify-between
                bg-[#292929]
                px-[25px]
                py-[15px]
                rounded-[25px]
                ${props.className}
            `} 
        >
            <p className="input-color">{props.link}</p>
            <button 
                onClick={() => navigator.clipboard.writeText(props.link)}
                className="h-[20px] w-[20px] focus:outline-none"
            >
                    <Image alt="Copy button" height="20px" width="20px" src="/assets/copy-icon.svg"/>
            </button>
        </div>
    );
}
