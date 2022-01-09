import App, { AppContext } from 'next/app'
import '../styles/globals.css'

export default function LinklyApp({ Component, pageProps }) {
  return <Component {...pageProps} />
}

LinklyApp.getInitialProps = async (ctx: AppContext) => {
    const appProps = await App.getInitialProps(ctx);
    return { ...appProps }
}
