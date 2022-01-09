module.exports = {
    reactStrictMode: true,
    publicRuntimeConfig: {
        backendURL: process.env.BACKEND_URL ?? "https://localhost:7057/api"
    }
}
