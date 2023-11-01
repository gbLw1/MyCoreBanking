import Header from "../Header";
import Sidebar from "../Sidebar";

type Props = {
  children: React.ReactNode;
};

export default function MainLayout({ children }: Props) {
  return (
    <div className="min-h-screen flex flex-col items-center justify-center bg-zinc-900">
      <Header />
      <Sidebar />
      {children}
    </div>
  );
}
