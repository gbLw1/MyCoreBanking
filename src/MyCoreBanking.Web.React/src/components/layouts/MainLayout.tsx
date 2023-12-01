import Header from "../Header";

type Props = {
  children: React.ReactNode;
};

export default function MainLayout({ children }: Props) {
  return (
    <>
      <Header />
      <div className="min-h-screen flex flex-col items-center justify-center bg-zinc-100 text-zinc-700">
        {children}
      </div>
    </>
  );
}
