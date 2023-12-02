import { Link } from "react-router-dom";
import Button from "./button";

export default function NotFound() {
  return (
    <div className="flex flex-col justify-center items-center h-screen bg-gradient-to-b from-sky-500 to-blue-500">
      <span className="text-4xl text-zinc-100">404</span>
      <span className="text-2xl text-zinc-100">Página não encontrada</span>

      <div
        className="w-[60%]
        bg-zinc-300 h-px my-8"
      />

      <Link to="/">
        <Button text="Voltar para a página inicial" variant="secondary" />
      </Link>
    </div>
  );
}
