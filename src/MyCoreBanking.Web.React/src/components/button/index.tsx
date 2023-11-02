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
        font-bold text-lg
        active:bg-blue-600 active:outline-none
        active:outline-none
        duration-300
      `,
        variant === "primary" &&
          `
            bg-blue-400 text-white
            hover:bg-blue-500 transition-colors
            active:bg-blue-600
          `,
        variant === "secondary" &&
          `
            bg-green-400 text-slate-800
            hover:bg-green-500 transition-colors
            active:bg-green-600
          `,
        disabled && "opacity-50 cursor-not-allowed"
      )}>
      {text}
    </button>
  );
}
