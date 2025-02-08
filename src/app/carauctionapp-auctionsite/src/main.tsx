import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import "./index.css";
import { ApplicationRouter } from "./ApplicationRouter.tsx";
import { ReactQueryClientProvider } from "./lib/react-query/ReactQueryClientProvider.tsx";

const rootElement = document.getElementById("root")!;
const reactRoot = createRoot(rootElement);
reactRoot.render(
  <StrictMode>
    <ReactQueryClientProvider>
      <ApplicationRouter />
    </ReactQueryClientProvider>
  </StrictMode>
);
