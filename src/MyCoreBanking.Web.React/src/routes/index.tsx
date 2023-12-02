import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import PrivateRoute from "../components/PrivateRoute";
import NotFound from "../components/NotFound";
import {
  Register,
  Home,
  Login,
  Transactions,
  Accounts,
  Profile,
  About,
} from "../pages";

export default function AppRouter() {
  return (
    <Router>
      <Routes>
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />

        <Route
          path="/"
          element={
            <PrivateRoute>
              <Home />
            </PrivateRoute>
          }
        />

        <Route
          path="/transactions"
          element={
            <PrivateRoute>
              <Transactions />
            </PrivateRoute>
          }
        />

        <Route
          path="/accounts"
          element={
            <PrivateRoute>
              <Accounts />
            </PrivateRoute>
          }
        />

        <Route
          path="/profile"
          element={
            <PrivateRoute>
              <Profile />
            </PrivateRoute>
          }
        />

        <Route
          path="/about"
          element={
            <PrivateRoute>
              <About />
            </PrivateRoute>
          }
        />

        <Route path="*" element={<NotFound />} />
      </Routes>
    </Router>
  );
}
