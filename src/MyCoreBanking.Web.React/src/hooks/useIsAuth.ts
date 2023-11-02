import AuthTokenModel from "../interfaces/models/AuthTokenModel";

export default function useIsAuth(): boolean {
  const auth: AuthTokenModel = JSON.parse(localStorage.getItem("auth") ?? "{}");

  if (auth.accessToken) {
    return true;
  }

  return false;
}
