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
          bg-blue-500 text-white
          hover:bg-blue-600 transition-colors duration-300
          active:bg-blue-700 active:outline-none
        `,
        variant === "secondary" &&
          `
            !bg-green-500 !text-slate-800
            hover:!bg-green-600
            active:!bg-green-700
          `,
        disabled && "opacity-50 cursor-not-allowed"
      )}>
      {text}
    </button>
  );
}
