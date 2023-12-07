import api from "./client/api"

export const getPerfil = async () => api.get('usuarios/perfil');

