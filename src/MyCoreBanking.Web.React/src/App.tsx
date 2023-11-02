import AppRouter from "./routes";
import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

export default function App() {
  return (
    <>
      <ToastContainer
        hideProgressBar={true}
        draggable={true}
        pauseOnHover={false}
        pauseOnFocusLoss={false}
      />
      <AppRouter />
    </>
  );
}
