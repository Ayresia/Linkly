import { DefaultSeo } from 'next-seo';
import App, { AppContext } from 'next/app'
import SEO from '../next-seo.config';
import '../styles/globals.css'

export default function LinklyApp({ Component, pageProps }) {
    return (
        <>
            <DefaultSeo {...SEO} />
            <Component {...pageProps} />
        </>
    );
}

LinklyApp.getInitialProps = async (ctx: AppContext) => {
    const appProps = await App.getInitialProps(ctx);
    return { ...appProps }
}
