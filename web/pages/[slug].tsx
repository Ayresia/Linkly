import { getSlugInfo } from "api/services/link";

export async function getServerSideProps(ctx) {
    let res = {
        redirect: {
            destination: '/',
            permanent: false
        }
    }

    try {
        let req = await getSlugInfo(ctx.params.slug);

        res.redirect.permanent = true;
        res.redirect.destination = req.url;
    } finally {
        return res;
    }
}

export default function RedirectUrl() {
    return <></>;
}
