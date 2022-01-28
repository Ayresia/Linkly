import https from 'https';
import axios from 'axios';
import getConfig from 'next/config';

const { publicRuntimeConfig } = getConfig();

const httpsAgent = new https.Agent({
    rejectUnauthorized: process.env.NODE_ENV == 'development' ? false : true
});

const axiosInstance = axios.create({
    baseURL: publicRuntimeConfig.backendURL,
    timeout: 5000,
    headers: { 'Content-Type': 'application/json' },
    httpsAgent: httpsAgent
});

axiosInstance.interceptors.request.use((request) =>
   request,
   async (error) => {
       return Promise.reject(error.response)
   },
);

axiosInstance.interceptors.response.use((response) =>
   response,
   async (error) => {
       return Promise.reject(error.response)
   },
);

const { get, post, put, delete: destroy } = axiosInstance;
export default { get, post, put, destroy };
