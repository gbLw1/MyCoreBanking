import { FaPiggyBank } from "react-icons/fa6";
import { Dropdown, Navbar } from "flowbite-react";
import { useNavigate } from "react-router-dom";

export default function Header() {
  const navigate = useNavigate();

  function Logout() {
    localStorage.clear();
    navigate("/login");
  }

  return (
    <Navbar fluid className="bg-slate-950 dark:bg-gray-800">
      <Navbar.Brand href="/">
        <FaPiggyBank
          className="text-yellow-200 rounded-full transform rotate-[-15deg] bg-black p-1.5 mr-3"
          size={32}
        />
        <span className="self-center whitespace-nowrap text-xl font-semibold dark:text-white">
          MyCoreBanking
        </span>
      </Navbar.Brand>
      <div className="flex md:order-2">
        <Dropdown
          arrowIcon={false}
          inline
          label={
            <span
              className="w-8 h-8 text-white font-semibold bg-blue-500 rounded-full
                flex items-center justify-center
                border border-blue-600
                dark:border-gray-700 dark:bg-gray-700 dark:text-gray-400">
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
        <Navbar.Link href="#" active>
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
