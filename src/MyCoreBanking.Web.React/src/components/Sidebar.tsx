import {
  FaBuildingColumns,
  FaChartLine,
  FaMoneyBillTransfer,
  FaPiggyBank,
  FaQuestion,
  FaUser,
} from "react-icons/fa6";

// 🔴🔴🔴 unused component 🔴🔴🔴
export default function Sidebar() {
  return (
    <nav className="bg-gradient-to-b from-blue-500 to-indigo-500 p-0 w-64">
      <div className="container-fluid flex flex-col p-0">
        <a
          className="navbar-brand flex justify-center items-center sidebar-brand m-0"
          href="/">
          <div className="sidebar-brand-icon rotate-n-15">
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
          </div>
          <div className="sidebar-brand-text mx-3 text-white">
            <span>
              <strong>MyCore</strong>
              <br />
              <strong>&nbsp;Banking</strong>
            </span>
          </div>
        </a>

        <hr className="sidebar-divider my-0" />

        <ul className="navbar-nav text-light" id="accordionSidebar">
          <li className="nav-item">
            <a
              className="nav-link d-flex flex-column flex-md-row justify-content-center justify-content-md-start align-items-center"
              href="/">
              <FaChartLine className="text-white" />
              <p
                className="m-0 mt-2 m-md-0 ms-md-1"
                style={{ lineHeight: "normal" }}>
                Home
              </p>
            </a>
          </li>

          <li className="nav-item">
            <a
              className="nav-link d-flex flex-column flex-md-row justify-content-center justify-content-md-start align-items-center"
              href="/transacoes">
              <FaMoneyBillTransfer className="text-white" />
              <p
                className="m-0 mt-2 m-md-0 ms-md-1"
                style={{ lineHeight: "normal" }}>
                Transações
              </p>
            </a>
          </li>

          <li className="nav-item">
            <a
              className="nav-link d-flex flex-column flex-md-row justify-content-center justify-content-md-start align-items-center"
              href="/contas">
              <FaBuildingColumns className="text-white" />
              <p
                className="m-0 mt-2 m-md-0 ms-md-1"
                style={{ lineHeight: "normal" }}>
                Contas
              </p>
            </a>
          </li>

          <li className="nav-item">
            <a
              className="nav-link d-flex flex-column flex-md-row justify-content-center justify-content-md-start align-items-center"
              href="/meu-perfil">
              <FaUser className="text-white" />
              <p
                className="m-0 mt-2 m-md-0 ms-md-1"
                style={{ lineHeight: "normal" }}>
                Perfil
              </p>
            </a>
          </li>

          <hr />

          <li className="nav-item">
            <a
              className="nav-link d-flex flex-column flex-md-row justify-content-center justify-content-md-start align-items-center"
              href="/sobre">
              <FaQuestion className="text-white" />
              <p
                className="m-0 mt-2 m-md-0 ms-md-1"
                style={{ lineHeight: "normal" }}>
                Sobre nós
              </p>
            </a>
          </li>
        </ul>
      </div>
    </nav>
  );
}
