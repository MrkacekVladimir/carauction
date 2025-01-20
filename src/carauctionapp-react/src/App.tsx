import "./App.css";
import { Outlet } from "react-router";

export function App() {
  return (
    <>
      <h1>Vite + React</h1>
      <Outlet />
    </>
  );
}
