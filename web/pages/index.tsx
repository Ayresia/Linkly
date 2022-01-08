import Button from '@components/button';
import ErrorMessage from '@components/errormessage';
import InputBox from '@components/inputbox';
import LinkPreview from '@components/linkpreview';
import { shortenUrl } from '@api/services/link';
import { Dispatch, MouseEvent, SetStateAction, useState } from 'react';

export const onClicked = async (event: MouseEvent<HTMLButtonElement>, setError: Dispatch<SetStateAction<string>>, setSlug: Dispatch<SetStateAction<string>>, url: string) => {
    event.preventDefault();

    if (url.trim().length == 0) {
        setError("Enter a valid URL");
        return;
    }

    let pattern = /^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$/;

    if (!url.match(pattern)) {
        setError("Enter a valid URL");
        return;
    }

    try {
        let resp = await shortenUrl(url);

        setError("")
        setSlug(`${location.href}${resp.slug}`);
    } catch (e) {
        if (e == undefined || e.data.trim().length == 0) {
            setError("Internal server error");
            return;
        }
    }
};

export default function Index() {
    const [url, setURL] = useState<string>("");
    const [error, setError] = useState<string>("");
    const [slug, setSlug] = useState<string>();

    return (
        <div className="h-full flex flex-col justify-center mx-auto w-82 sm:w-[500px] gap-[25px]">
                <div className="flex flex-col items-center gap-[25px]">
                    <img className="h-16" alt="Linkly Logo" src="/assets/logo.svg"/>
                    <p className="opacity-30 sm:text-[22px] text-white text-center">Shorten your links, on your own terms.</p>
                </div>
                <div className="flex flex-col sm:flex-row gap-[25px] sm:gap-0">
                    <InputBox 
                        onChange={e => setURL(e.target.value)}
                        className="rounded-[25px] sm:rounded-none sm:rounded-tl-[25px] sm:rounded-bl-[25px] flex-grow"
                        placeholder="https://example.com"
                    />
                    <Button
                        onClick={(e: MouseEvent<HTMLButtonElement>) => onClicked(e, setError, setSlug, url)}
                        className="rounded-[25px] sm:rounded-none sm:rounded-tr-[25px] sm:rounded-br-[25px]">Shorten
                    </Button>
                </div>
                { slug && <LinkPreview link={slug} /> }
                { error && <ErrorMessage error={error} setError={setError} /> }
        </div>
    )
}
