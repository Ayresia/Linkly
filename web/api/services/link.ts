import { ShortenUrlResponse, UrlInterface } from '@types';
import axiosClient from '../axios';

export async function getSlugInfo(slug: string) {
    let req = await axiosClient.get<UrlInterface>(`/info/${slug}`);
    return req.data;
}

export async function shortenUrl(url: string) {
    let req = await axiosClient.post<ShortenUrlResponse>(`/shorten`, { url: url });
    return req.data;
}

export default { getSlugInfo, shortenUrl };
