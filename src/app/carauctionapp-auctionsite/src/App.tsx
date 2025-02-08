import "./App.css";
import { Outlet } from "react-router";
import { NavigationBar } from "./components/navigation/NavigationBar";

export function App() {
  return (
    <>
      <NavigationBar />
      <Outlet />
    </>
  );
}
