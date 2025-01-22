import "./App.css";
import { Outlet } from "react-router";

export function App() {
  return (
    <>
      <h1>test</h1>
      <Outlet />
    </>
  );
}
