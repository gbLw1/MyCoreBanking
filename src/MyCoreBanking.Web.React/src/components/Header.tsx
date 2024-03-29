import { FaHome } from "react-icons/fa";
import {
  FaMoneyBillTransfer,
  FaPiggyBank,
  FaCircleInfo,
} from "react-icons/fa6";
import { MdAccountBalance, MdOutlineLogout } from "react-icons/md";
import { IoPerson } from "react-icons/io5";
import { Link, useLocation, useNavigate } from "react-router-dom";
import clsx from "clsx";
import { useEffect, useRef, useState } from "react";
import { getPerfil } from "../services/user.service";
import toast from "react-hot-toast";
import { Spinner } from "flowbite-react";
import UsuarioModel from "../interfaces/models/UsuarioModel";
import { useUser } from "../contexts/UserContext";

interface Page {
  name: string;
  path: string;
  icon: JSX.Element;
}

const pages: Page[] = [
  {
    name: "Home",
    path: "/",
    icon: <FaHome />,
  },
  {
    name: "Transações",
    path: "/transactions",
    icon: <FaMoneyBillTransfer />,
  },
  {
    name: "Contas",
    path: "/accounts",
    icon: <MdAccountBalance />,
  },
  {
    name: "Perfil",
    path: "/profile",
    icon: <IoPerson />,
  },
  {
    name: "Sobre nós",
    path: "/about",
    icon: <FaCircleInfo />,
  },
];

export default function Header() {
  const location = useLocation();
  const path = location.pathname;
  const navigate = useNavigate();

  const { setUser, nome } = useUser();

  const [loading, setLoading] = useState<boolean>(false);
  const [dropdownOpen, setDropdownOpen] = useState<boolean>(false);
  const dropdownTargetRef = useRef<HTMLDivElement>(null);
  const dropdownRef = useRef<HTMLDivElement>(null);

  function Logout() {
    localStorage.clear();
    navigate("/login");
  }

  async function getUserData() {
    if (nome) {
      return;
    }

    const auth = localStorage.getItem("auth");

    if (!auth) {
      return;
    }

    setLoading(true);

    getPerfil()
      .then(({ data }) => {
        const user: UsuarioModel = data;
        setUser(user);
      })
      .catch((err) => {
        toast.error(
          err.response?.data?.message || "Erro ao carregar dados do usuário"
        );
      })
      .finally(() => {
        setLoading(false);
      });
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

  useEffect(() => {
    getUserData();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  return (
    <header
      className="fixed px-3 py-2 w-full bg-gradient-to-b from-sky-500 to-blue-500
      shadow-sm z-10 flex flex-wrap justify-between items-center">
      <Link to="/" className="flex items-center">
        <FaPiggyBank
          className="text-amber-400
            border-2 border-amber-400
            rounded-full
            transform rotate-[-15deg] 
            p-1 mr-2 lg:mr-1
          "
          size={40}
        />
        <span className="text-white self-center whitespace-nowrap text-lg lg:text-xl font-normal">
          MyCoreBanking
        </span>
      </Link>

      {/* Page links */}
      <div className="hidden lg:flex -ms-36 items-center gap-3">
        {pages.map((page) => (
          <Link
            key={page.path}
            to={page.path}
            className={clsx(
              "text-white self-center whitespace-nowrap text-sm lg:text-base font-normal transition-all duration-100 hover:scale-110",
              path === page.path &&
                "!font-bold border-2 border-white rounded-full px-2 py-1 tracking-wider"
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
          className="leading-4 w-9 h-9 font-bold bg-transparent rounded-full flex items-center justify-center border-2 border-white text-2xl text-white cursor-pointer">
          {loading ? <Spinner /> : nome?.[0]?.toUpperCase() || "?"}
        </span>

        <div
          ref={dropdownRef}
          className={clsx(
            "absolute top-0 right-0 flex-col items-center justify-center rounded-md border-2 border-white mt-12 w-36 lg:w-28 py-2 px-4 bg-white text-black shadow-md z-10 transition-all duration-200",
            dropdownOpen ? "opacity-100" : "opacity-0 pointer-events-none"
          )}>
          <div className="absolute top-0 right-0 w-3 h-3 bg-white transform rotate-45 -translate-y-1 -translate-x-2 -mt-1" />

          {/* 🟡📱 Mobile 📱🟡 -> Page links */}
          <div className="flex lg:hidden flex-col gap-2">
            {pages.map((page) => (
              <Link
                key={page.path}
                to={page.path}
                className={clsx(
                  "flex items-center justify-between text-zinc-700 whitespace-nowrap text-sm lg:text-md font-normal",
                  path === page.path && "!text-sky-500 !font-bold"
                )}>
                <span className="me-2">{page.icon}</span>
                {page.name}
              </Link>
            ))}
            <hr className="w-full border-gray-300 my-2" />
          </div>

          <span
            onClick={Logout}
            className="flex justify-between items-center cursor-pointer">
            <MdOutlineLogout className="me-2" />
            Sair
          </span>
        </div>
      </div>
    </header>
  );
}
