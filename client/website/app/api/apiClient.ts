import axios from 'axios';
import apiRoutes from "./apiEndpoints.config";

const apiClient = axios.create({
    baseURL: apiRoutes.baseUrl,
    headers: {
        "Content-Type": "application/json",
    },
});

apiClient.interceptors.request.use(
    (config) => {
        const token = localStorage.getItem("token");
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    },
    (error) => {
        return Promise.reject(error);
    }
);

apiClient.interceptors.response.use(
    response => response,
    error => {
        if (error.response) {
            if (error.response.status === 401) {
                console.log("Unauthorized access");
            }
        } else {
            console.error(error.message);
        }
        return Promise.reject(error);
    }
);

export default apiClient;
