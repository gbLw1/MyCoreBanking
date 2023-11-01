import YellowLogo from "../YellowLogo";

type Props = {
  children: React.ReactNode;
};

export default function CleanLayout({ children }: Props) {
  return (
    <div className="h-screen w-screen bg-blue-500 flex items-center justify-center">
      <div className="max-w-[600px] w-full p-8 gap-4 bg-white rounded-md shadow-md flex flex-col justify-start">
        <header className="flex flex-col justify-center items-center gap-4">
          <YellowLogo />
        </header>
        {children}
      </div>
    </div>
  );
}
