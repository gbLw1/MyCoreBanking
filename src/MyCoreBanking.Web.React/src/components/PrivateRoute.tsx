import { ReactNode, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import toast from "react-hot-toast";
import useIsAuth from "../hooks/useIsAuth";

type Props = {
  children: ReactNode;
};

export default function PrivateRoute({ children }: Props) {
  const navigate = useNavigate();
  const isAuth: boolean = useIsAuth();

  useEffect(() => {
    const currentLocation = window.location.pathname;

    if (!isAuth && currentLocation !== "/login") {
      navigate("/login");
      toast.error("Você deve realizar o login para acessar essa página.");
    }
  }, [isAuth, navigate]);

  return (
    <>
      {!isAuth && null}
      {isAuth && children}
    </>
  );
}
