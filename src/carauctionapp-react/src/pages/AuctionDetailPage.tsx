import { apiUrl } from "@/lib/api/config";
import { useQuery } from "@tanstack/react-query";
import { useParams } from "react-router";
import { AuctionBiddingBook } from "./AuctionBiddingBook";

export function AuctionDetailPage() {
  const { auctionId } = useParams();
  const {
    data: auction,
    isPending,
    isError,
  } = useQuery({
    queryKey: ["auctions", auctionId],
    queryFn: async () => {
      const response = await fetch(`${apiUrl}/auctions/${auctionId}`);
      return await response.json();
    },
  });

  if (isError || auctionId === undefined) {
    return <div>Failed to fetch auction...</div>;
  }

  if (isPending) {
    return <div>Loading auction...</div>;
  }

  return (
    <div>
      <h1>Auction Detail {auctionId}</h1>
      <AuctionBiddingBook auctionId={auctionId} />
    </div>
  );
}
