import { useAuctionListQuery } from "./useAuctionListQuery";
import { AuctionCard } from "@/components/auction-card/AuctionCard";

export function AuctionListPage() {
  const { data, isPending, isError } = useAuctionListQuery();

  if (isError) {
    return <div>Failed to load auction list...</div>;
  }

  if (isPending) {
    return <div>Loading auctions...</div>;
  }

  const auctions = data.auctions;

  return (
    <div>
      <h1>Auction List Page</h1>
      <div>
        {auctions.map((a) => (
          <AuctionCard key={a.id} auction={a} />
        ))}
      </div>
    </div>
  );
}
