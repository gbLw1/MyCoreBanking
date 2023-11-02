import axios, { CreateAxiosDefaults } from "axios";
import AuthTokenModel from "../interfaces/models/AuthTokenModel";
import { toast } from "react-toastify";

const defaultOptions: CreateAxiosDefaults = {
  baseURL: import.meta.env.VITE_BASE_API_URL,
  headers: {
    "Content-Type": "application/json",
  },
};

const api = axios.create(defaultOptions);

api.interceptors.request.use((config) => {
  const auth: AuthTokenModel = JSON.parse(localStorage.getItem("auth") ?? "{}");

  if (auth.accessToken) {
    config.headers.Authorization = `Bearer ${auth.accessToken}`;
  }

  return config;
});

api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response && error.response.status === 401) {
      toast.error("Sua sessão expirou. Faça login novamente.", {
        autoClose: 5000,
      });

      localStorage.removeItem("auth");
      window.location.href = "/login";
    }

    return Promise.reject(error);
  }
);

export default api;
