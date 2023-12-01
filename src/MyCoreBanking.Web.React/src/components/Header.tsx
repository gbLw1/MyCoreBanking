import { FaPiggyBank } from "react-icons/fa6";
import { CiLogout } from "react-icons/ci";
import { Link, useLocation, useNavigate } from "react-router-dom";
import clsx from "clsx";
import { useEffect, useRef, useState } from "react";

interface Page {
  name: string;
  path: string;
}

const pages: Page[] = [
  {
    name: "Home",
    path: "/",
  },
  {
    name: "Transações",
    path: "/transactions",
  },
  {
    name: "Contas",
    path: "/accounts",
  },
  {
    name: "Perfil",
    path: "/profile",
  },
  {
    name: "Sobre nós",
    path: "/about",
  },
];

export default function Header() {
  const location = useLocation();
  const path = location.pathname;
  const navigate = useNavigate();

  const [dropdownOpen, setDropdownOpen] = useState<boolean>(false);
  const dropdownTargetRef = useRef<HTMLDivElement>(null);
  const dropdownRef = useRef<HTMLDivElement>(null);

  function Logout() {
    localStorage.clear();
    navigate("/login");
  }

  useEffect(() => {
    const closeDropdownOnOverlayClick = (event: MouseEvent) => {
      if (
        !dropdownRef.current?.contains(event.target as Node) &&
        !dropdownTargetRef.current?.contains(event.target as Node)
      ) {
        setDropdownOpen(false);
      }
    };

    document.addEventListener("mousedown", closeDropdownOnOverlayClick);

    return () => {
      document.removeEventListener("mousedown", closeDropdownOnOverlayClick);
    };
  });

  return (
    <header className="fixed px-3 py-2 w-full bg-white shadow-sm z-10 flex flex-wrap justify-between items-center">
      <div className="flex items-center">
        <FaPiggyBank
          className="text-yellow-400 
            bg-zinc-50
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
      </div>

      {/* Page links */}
      <div className="hidden md:flex items-center gap-3">
        {pages.map((page) => (
          <Link
            key={page.path}
            to={page.path}
            className={clsx(
              "text-zinc-700 self-center whitespace-nowrap text-sm md:text-md font-normal",
              path === page.path && "!text-cyan-700 !font-bold"
            )}>
            {page.name}
          </Link>
        ))}
      </div>

      {/* Dropdown */}
      <div className="relative flex items-center">
        <span
          ref={dropdownTargetRef}
          onClick={() => setDropdownOpen(!dropdownOpen)}
          className="me-1 md:me-0 w-8 h-8 
                text-white font-normal 
                bg-transparent rounded-full
                flex items-center justify-center
                border-2 border-cyan-700
                text-lg
                text-zinc-700 
                cursor-pointer
              ">
          G
        </span>

        <div
          ref={dropdownRef}
          className={clsx(
            "absolute top-0 right-0 flex-col items-center justify-center rounded-md mt-10 border-2 border-zinc-300 me-2 w-36 md:w-28 p-2 bg-white text-black shadow-md z-10 transition-all duration-200",
            dropdownOpen ? "opacity-100" : "opacity-0 pointer-events-none"
          )}>
          {/* 🟡📱 Mobile 📱🟡 -> Page links */}
          <div className="flex md:hidden flex-col items-center justify-center gap-2">
            {pages.map((page) => (
              <Link
                key={page.path}
                to={page.path}
                className={clsx(
                  "text-zinc-700 self-center whitespace-nowrap text-sm md:text-md font-normal",
                  path === page.path && "!text-cyan-700 !font-bold"
                )}>
                {page.name}
              </Link>
            ))}
            <hr className="w-full border-gray-300 my-1" />
          </div>

          <span
            onClick={Logout}
            className="flex justify-center items-center cursor-pointer hover:text-cyan-700">
            <CiLogout className="me-2" />
            Sair
          </span>
        </div>
      </div>
    </header>
  );
}
