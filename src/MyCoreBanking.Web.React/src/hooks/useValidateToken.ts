import { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import useIsAuth from "./useIsAuth";

export default function useValidateToken() {
  const navigate = useNavigate();
  const isAuth: boolean = useIsAuth();

  useEffect(() => {
    if (isAuth) {
      navigate("/");
    }
  }, [isAuth, navigate]);
}
