import axios, { CreateAxiosDefaults } from "axios";

const defaultOptions: CreateAxiosDefaults = {
  baseURL: import.meta.env.VITE_BASE_API_URL,
  headers: {
    "Content-Type": "application/json",
  },
};

const api = axios.create(defaultOptions);

export default api;
