import { Link, useNavigate } from "react-router-dom";
import CleanLayout from "../../components/layouts/CleanLayout";
import api from "../../services/api";
import AuthTokenPostArgs from "../../interfaces/args/AuthTokenPostArgs";
import ApiErrorHandler from "../../services/apiErrorHandler";
import { useForm } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import * as yup from "yup";
import Input from "../../components/input";
import Button from "../../components/button";
import { AxiosError } from "axios";
import toast from "react-hot-toast";
import useValidateToken from "../../hooks/useValidateToken";
import { useState } from "react";

const AuthTokenPostArgsValidation = yup.object().shape({
  email: yup.string().email("E-mail inválido").required("Informe o e-mail"),
  senha: yup.string().required("Informe a senha"),
});

export default function Login() {
  const navigate = useNavigate();

  useValidateToken();

  const {
    control,
    handleSubmit,
    formState: { errors },
  } = useForm<AuthTokenPostArgs>({
    resolver: yupResolver<AuthTokenPostArgs>(AuthTokenPostArgsValidation),
  });

  const [loading, setLoading] = useState<boolean>(false);

  async function Login(data: AuthTokenPostArgs): Promise<void> {
    setLoading(true);

    toast.promise(
      api.post("/auth/token", data).finally(() => setLoading(false)),
      {
        loading: "Entrando...",
        success: ({ data }) => {
          localStorage.setItem("auth", JSON.stringify(data));
          navigate("/");
          return "Bem vindo!";
        },
        error: (error: Error | AxiosError) => ApiErrorHandler(error),
      }
    );
  }

  return (
    <CleanLayout>
      <div className="flex-1">
        <span className="text-black text-2xl flex justify-center">
          Bem vindo de volta!
        </span>

        <form
          onSubmit={handleSubmit(Login)}
          className="gap-4 flex flex-col mt-8">
          <Input
            control={control}
            fieldError={errors.email}
            name="email"
            type="email"
            placeholder="E-mail"
          />

          <Input
            control={control}
            fieldError={errors.senha}
            name="senha"
            type="password"
            placeholder="Senha"
          />

          <Button type="submit" disabled={loading} text="Entrar" />
        </form>

        <div className="w-full bg-zinc-300 h-px my-8" />

        <Link
          to="/register"
          className="text-blue-500 underline underline-offset-1 flex justify-center">
          Criar uma nova conta
        </Link>
      </div>
    </CleanLayout>
  );
}
