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
      disabled={disabled}
      className={clsx(
        "py-3 px-4 rounded-lg font-bold text-lg bg-blue-500 text-white hover:bg-blue-600 transition-colors duration-300 active:bg-blue-700 active:outline-none",
        variant === "secondary" &&
          "border-2 border-blue-500 !bg-white !text-blue-500 hover:scale-105 active:scale-95 transition-transform duration-75",
        disabled && "opacity-50 cursor-not-allowed"
      )}>
      {text}
    </button>
  );
}
