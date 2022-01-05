import https from 'https';
import axios from 'axios';
import { ErrorResponse } from '@types';

const httpsAgent = new https.Agent({
    rejectUnauthorized: process.env.NODE_ENV == 'development' ? false : true
});

const axiosInstance = axios.create({
    baseURL: "https://localhost:7057/api",
    timeout: 1000,
    headers: { 'Content-Type': 'application/json' },
    httpsAgent: httpsAgent
});

axiosInstance.interceptors.request.use((request) =>
   request,
   async (error) => {
       return Promise.reject(error.response.data)
   },
);

axiosInstance.interceptors.response.use((response) =>
   response,
   async (error) => {
       return Promise.reject(error.response.data)
   },
);

const { get, post, put, delete: destroy } = axiosInstance;
export default { get, post, put, destroy };
