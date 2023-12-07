import {
  Dispatch,
  SetStateAction,
  createContext,
  useContext,
  useState,
} from "react";
import UsuarioModel from "../interfaces/models/UsuarioModel";

interface UserContextProps extends UsuarioModel {
  setUser: Dispatch<SetStateAction<UsuarioModel>>;
}

const UserContext = createContext<UserContextProps>({} as UserContextProps);

export const UserProvider = ({ children }: { children: React.ReactNode }) => {
  const [user, setUser] = useState({
    id: "",
    nome: "",
    email: "",
  });

  return (
    <UserContext.Provider value={{ ...user, setUser }}>
      {children}
    </UserContext.Provider>
  );
};

export const useUser = () => {
  return useContext(UserContext);
};
