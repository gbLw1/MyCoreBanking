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
    <Navbar fluid className="bg-zinc-950">
      <Navbar.Brand href="/">
        <FaPiggyBank
          className="text-yellow-200 rounded-full transform rotate-[-15deg] bg-zinc-950 p-1.5 mr-3"
          size={40}
        />
        <span className="self-center whitespace-nowrap text-xl font-normal">
          MyCoreBanking
        </span>
      </Navbar.Brand>
      <div className="flex md:order-2">
        <Dropdown
          arrowIcon={false}
          inline
          label={
            <span
              className="me-3 md:me-0 w-8 h-8 text-white font-normal bg-transparent rounded-full
                flex items-center justify-center
                border-2 border-cyan-700
                text-lg
              ">
              G
            </span>
          }>
          <Dropdown.Header>
            <span className="block text-sm">gbL</span>
            <span className="block truncate text-sm font-medium">
              teste@teste.com
            </span>
          </Dropdown.Header>
          <Dropdown.Item>Dashboard</Dropdown.Item>
          <Dropdown.Item>Settings</Dropdown.Item>
          <Dropdown.Item>Earnings</Dropdown.Item>
          <Dropdown.Divider />
          <Dropdown.Item onClick={Logout}>Sign out</Dropdown.Item>
        </Dropdown>
        <Navbar.Toggle />
      </div>
      <Navbar.Collapse>
        <Navbar.Link href="/" active={location.pathname === "/"}>
          Home
        </Navbar.Link>
        <Navbar.Link href="#">About</Navbar.Link>
        <Navbar.Link href="#">Services</Navbar.Link>
        <Navbar.Link href="#">Pricing</Navbar.Link>
        <Navbar.Link href="#">Contact</Navbar.Link>
      </Navbar.Collapse>
    </Navbar>
  );
}
