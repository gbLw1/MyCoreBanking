import { Link, useNavigate } from "react-router-dom";
import CleanLayout from "../../components/layouts/CleanLayout";
import api from "../../services/api";
import UsuariosPostArgs from "../../interfaces/args/UsuariosPostArgs";
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

const UsuariosPostArgsValidation = yup.object().shape({
  nome: yup.string().required("Informe o nome"),
  email: yup.string().email("E-mail inválido").required("Informe o e-mail"),
  senha: yup.string().required("Informe a senha"),
  confirmarSenha: yup
    .string()
    .oneOf([yup.ref("senha")], "As senhas devem ser iguais")
    .required("Confirme a senha"),
});

export default function Register() {
  const navigate = useNavigate();

  useValidateToken();

  const {
    control,
    handleSubmit,
    formState: { errors },
  } = useForm<UsuariosPostArgs>({
    resolver: yupResolver<UsuariosPostArgs>(UsuariosPostArgsValidation),
  });

  const [loading, setLoading] = useState<boolean>(false);

  async function Cadastrar(data: UsuariosPostArgs): Promise<void> {
    setLoading(true);

    toast.promise(
      api.post("/usuarios", data).finally(() => setLoading(false)),
      {
        loading: "Cadastrando...",
        success: () => {
          navigate("/login");
          return "Cadastrado com sucesso!";
        },
        error: (error: Error | AxiosError) => ApiErrorHandler(error),
      }
    );
  }

  return (
    <CleanLayout>
      <div className="flex-1">
        <span className="text-black text-2xl flex justify-center">
          Crie sua conta
        </span>

        <form
          onSubmit={handleSubmit(Cadastrar)}
          className="gap-4 flex flex-col mt-8">
          <Input
            control={control}
            fieldError={errors.nome}
            name="nome"
            type="text"
            placeholder="Nome completo"
          />

          <Input
            control={control}
            fieldError={errors.email}
            name="email"
            type="email"
            placeholder="E-mail"
          />

          <div className="flex gap-4">
            <Input
              control={control}
              fieldError={errors.senha}
              name="senha"
              type="password"
              placeholder="Senha"
            />

            <Input
              control={control}
              fieldError={errors.confirmarSenha}
              name="confirmarSenha"
              type="password"
              placeholder="Confirmar senha"
            />
          </div>

          <Button type="submit" text="Criar conta" disabled={loading} />
        </form>

        <div className="w-full bg-zinc-300 h-px my-8" />

        <Link
          to="/login"
          className="text-blue-500 underline underline-offset-1 flex justify-center">
          Já possui uma conta? Faça o login
        </Link>
      </div>
    </CleanLayout>
  );
}
