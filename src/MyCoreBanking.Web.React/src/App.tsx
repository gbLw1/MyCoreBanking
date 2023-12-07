import { Toaster } from "react-hot-toast";
import AppRouter from "./routes";
import { UserProvider } from "./contexts/UserContext";

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
      <UserProvider>
        <AppRouter />
      </UserProvider>
    </>
  );
}
