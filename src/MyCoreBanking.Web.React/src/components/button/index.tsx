import clsx from "clsx";
import { ComponentProps } from "react";

interface Props extends ComponentProps<"button"> {
  text: string;
  variant?: "primary" | "secondary";
}

export default function Button({
  text,
  variant = "primary",
  type = "button",
  onClick,
  disabled,
}: Props) {
  return (
    <button
      type={type}
      onClick={onClick}
      className={clsx(
        `
        p-3 rounded-lg 
        text-white font-bold text-lg
        hover:bg-blue-500 transition-colors duration-200
        active:bg-blue-600 active:outline-none
      `,
        variant === "primary" && "bg-blue-400",
        variant === "secondary" && "bg-green-400",
        disabled && "opacity-50 cursor-not-allowed"
      )}>
      {text}
    </button>
  );
}
