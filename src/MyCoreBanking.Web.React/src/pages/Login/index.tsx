import { Link } from "react-router-dom";
import CleanLayout from "../../components/layouts/CleanLayout";
import api from "../../services/api";
import AuthTokenPostArgs from "../../interfaces/args/AuthTokenPostArgs";
import { ToastContentProps, toast } from "react-toastify";
import ApiErrorHandler from "../../services/apiErrorHandler";

export default function Login() {
  // todo: react-hook-form
  function handleLogin(event: React.FormEvent) {
    event.preventDefault();

    const args: AuthTokenPostArgs = {
      email: "",
      senha: "",
    };

    toast.promise(api.post("/auth/token", args), {
      pending: "Entrando...",
      success: "Bem vindo!",
      error: {
        render({ data }: ToastContentProps) {
          return ApiErrorHandler(data);
        },
      },
    });
  }

  return (
    <CleanLayout>
      <div className="flex-1">
        <span className="text-black text-2xl flex justify-center">
          Bem vindo de volta!
        </span>

        <form onSubmit={handleLogin} className="gap-4 flex flex-col mt-8">
          <input
            type="text"
            name="username"
            id="username"
            placeholder="E-mail"
            className="text-black p-4 rounded-full border border-zinc-300"
          />
          <input
            type="password"
            name="password"
            id="password"
            placeholder="Senha"
            className="text-black p-4 rounded-full border border-zinc-300"
          />

          <button
            type="submit"
            className="bg-blue-400 text-white p-4 rounded-full font-bold">
            Entrar
          </button>
        </form>

        <div className="w-full bg-zinc-300 h-px my-8" />

        <Link
          to="/cadastro"
          className="text-blue-500 underline underline-offset-1 flex justify-center">
          Criar uma nova conta
        </Link>
      </div>
    </CleanLayout>
  );
}
