import Header from "../Header";

type Props = {
  children: React.ReactNode;
};

export default function MainLayout({ children }: Props) {
  return (
    <>
      <Header />
      <div className="min-h-screen pt-[72px] p-4 bg-zinc-200 text-zinc-700">
        {children}
      </div>
    </>
  );
}
