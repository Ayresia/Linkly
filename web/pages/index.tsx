import Button from '@components/button';
import InputBox from '@components/inputbox';

export default function Index() {
    return (
        <div className="h-full flex flex-col justify-center mx-auto w-82 sm:w-[500px] gap-[25px]">
                <div className="flex flex-col items-center gap-[25px]">
                    <img className="h-16" alt="Linkly Logo" src="/assets/logo.svg"/>
                    <p className="opacity-30 sm:text-[22px] text-white text-center">Shorten your links, on your own terms.</p>
                </div>
                <div className="flex flex-col sm:flex-row gap-[25px] sm:gap-0">
                    <InputBox 
                        className="rounded-[25px] sm:rounded-none sm:rounded-tl-[25px] sm:rounded-bl-[25px] flex-grow"
                        placeholder="https://example.com"
                    />
                    <Button className="rounded-[25px] sm:rounded-none sm:rounded-tr-[25px] sm:rounded-br-[25px]">Shorten</Button>
                </div>
        </div>
    )
}
