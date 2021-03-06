import { LinkPreviewProps } from '@types';
import Image from 'next/image';

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
                animate-fade-in-down
                ${props.className}
            `} 
        >
            <p className="input-color overflow-x-auto w-[89%]">{props.link}</p>
            <button 
                onClick={() => navigator.clipboard.writeText(props.link)}
                className="h-[20px] w-[20px] focus:outline-none"
            >
                    <Image alt="Copy button" height="20px" width="20px" src="/assets/copy-icon.svg"/>
            </button>
        </div>
    );
}
