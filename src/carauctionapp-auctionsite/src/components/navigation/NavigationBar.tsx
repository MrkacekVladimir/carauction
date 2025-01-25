import { NavLink } from "react-router";
import styles from "./NavigationBar.module.css";

const routes = [
  { path: "/auctions", name: "Auctions" },
  { path: "/auctions/create", name: "New Auction" },
];

export function NavigationBar() {
  return (
    <nav className={styles.navbar}>
      <ul className={styles.navList}>
        {routes.map((route) => (
          <li key={route.path} className={styles.navItem}>
            <NavLink
              end
              to={route.path}
              className={({ isActive }) =>
                isActive ? styles.activeLink : styles.navLink
              }
            >
              {route.name}
            </NavLink>
          </li>
        ))}
      </ul>
    </nav>
  );
}
