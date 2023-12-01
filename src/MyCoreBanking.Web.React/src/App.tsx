import { Toaster } from "react-hot-toast";
import AppRouter from "./routes";

export default function App() {
  return (
    <>
      <Toaster
        position="top-center"
        reverseOrder={false}
        toastOptions={{
          duration: 3000,
          style: {
            background: "#363636",
            color: "#fff",
          },
        }}
      />
      <AppRouter />
    </>
  );
}
