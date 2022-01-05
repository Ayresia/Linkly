export interface UrlInterface {
    url: string
}

export interface ShortenUrlResponse {
    slug: string
}

export interface ErrorResponse {
    status: number,
    error: string
}
