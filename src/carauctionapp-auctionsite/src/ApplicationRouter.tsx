import { BrowserRouter, Route, Routes } from "react-router";
import { App } from "./App";
import { AuctionListPage } from "./pages/AuctionListPage";
import { AuctionDetailPage } from "./pages/AuctionDetailPage";

export function ApplicationRouter() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<App />}>
          <Route path="auctions">
            <Route index element={<AuctionListPage />} />
            <Route path=":auctionId" element={<AuctionDetailPage />} />
          </Route>
        </Route>
      </Routes>
    </BrowserRouter>
  );
}
