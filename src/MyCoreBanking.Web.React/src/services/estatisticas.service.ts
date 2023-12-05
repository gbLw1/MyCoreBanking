import api from "./client/api";

export const getEstatisticas = async () => api.get("/relatorios");
