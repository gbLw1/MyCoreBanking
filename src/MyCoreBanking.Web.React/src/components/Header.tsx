import { FaPiggyBank } from "react-icons/fa6";
import { Dropdown, Navbar } from "flowbite-react";
import { useLocation, useNavigate } from "react-router-dom";

export default function Header() {
  const location = useLocation();
  const navigate = useNavigate();

  function Logout() {
    localStorage.clear();
    navigate("/login");
  }

  return (
    <Navbar fluid className="fixed w-full">
      <Navbar.Brand href="/">
        <FaPiggyBank
          className="text-yellow-400 
            border-2 border-yellow-400 
            rounded-full
            transform rotate-[-15deg] 
            p-1 mr-2 md:mr-3
          "
          size={40}
        />
        <span className="text-zinc-700 self-center whitespace-nowrap text-lg md:text-xl font-normal">
          MyCoreBanking
        </span>
      </Navbar.Brand>

      <div className="flex md:order-2">
        <Dropdown
          arrowIcon={false}
          inline
          label={
            <span
              className="me-1 md:me-0 w-8 h-8 text-white font-normal bg-transparent rounded-full
                flex items-center justify-center
                border-2 border-cyan-700
                text-lg
                text-zinc-700 
              ">
              G
            </span>
          }>
          <Dropdown.Header>
            <span className="block text-sm font-bold mb-2">gbL</span>
            <span className="block truncate text-sm font-medium">
              teste@teste.com
            </span>
          </Dropdown.Header>
          <Dropdown.Item onClick={Logout}>Sair</Dropdown.Item>
        </Dropdown>
        <Navbar.Toggle />
      </div>

      <Navbar.Collapse>
        <Navbar.Link href="/" active={location.pathname === "/"}>
          Home
        </Navbar.Link>
        <Navbar.Link
          href="/transactions"
          active={location.pathname === "/transactions"}>
          Transações
        </Navbar.Link>
        <Navbar.Link
          href="/accounts"
          active={location.pathname === "/accounts"}>
          Contas
        </Navbar.Link>
        <Navbar.Link href="/profile" active={location.pathname === "/profile"}>
          Perfil
        </Navbar.Link>
        <Navbar.Link href="/about" active={location.pathname === "/about"}>
          Sobre nós
        </Navbar.Link>
      </Navbar.Collapse>
    </Navbar>
  );
}
