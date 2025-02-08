import { BrowserRouter, Navigate, Route, Routes } from "react-router";
import { App } from "./App";
import { AuctionListPage } from "./pages/auction-list/AuctionListPage";
import { AuctionDetailPage } from "./pages/auction-detail/AuctionDetailPage";
import { CreateAuctionPage } from "./pages/create-auction/CreateAuctionPage";

export function ApplicationRouter() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<App />}>
          <Route index element={<Navigate to={"auctions"} />} />
          <Route path="auctions">
            <Route index element={<AuctionListPage />} />
            <Route path="create" element={<CreateAuctionPage />} />
            <Route path=":auctionId" element={<AuctionDetailPage />} />
          </Route>
        </Route>
      </Routes>
    </BrowserRouter>
  );
}
