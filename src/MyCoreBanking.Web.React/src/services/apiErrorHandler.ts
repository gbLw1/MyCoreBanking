import { isAxiosError } from "axios";

export default function ApiErrorHandler(error: object | undefined): string {
  if (
    isAxiosError(error) &&
    error.response &&
    error.response.status >= 400 &&
    error.response.status < 500
  ) {
    return error.response.data;
  }

  return "Ocorreu um erro inesperado. Tente novamente mais tarde.";
}
