import clsx from "clsx";
import { Control, Controller, FieldError } from "react-hook-form";

type Props = {
  defaultValue?: string;
  placeholder: string;
  name: string;
  type?: "text" | "password" | "email";
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  control: Control<any>;
  disabled?: boolean;
  fieldError?: FieldError;
};

export default function Input({
  defaultValue = "",
  placeholder,
  name,
  type = "text",
  control,
  disabled,
  fieldError,
}: Props) {
  return (
    <div className="flex flex-col gap-1 w-full">
      <Controller
        control={control}
        name={name}
        defaultValue={defaultValue}
        render={({ field }) => (
          <input
            {...field}
            type={type}
            placeholder={placeholder}
            className={clsx(
              `
                text-black
                p-2 rounded-lg
                border border-zinc-300
                focus:outline-none focus:border-blue-400
                transition-colors duration-200
              `,
              fieldError && "!border-red-500"
            )}
            disabled={disabled}
          />
        )}
      />

      {fieldError && (
        <span className="text-red-500 text-sm">
          {fieldError.message?.toString()}
        </span>
      )}
    </div>
  );
}
