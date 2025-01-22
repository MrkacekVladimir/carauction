import { Link } from "react-router";
import { useAuctionListQuery } from "./useAuctionListQuery";

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
          <section key={a.id}>
            <div>{a.title}</div>
            <div>Bids: {a.bids.length}</div>
            <Link to={a.id}>To auction</Link>
          </section>
        ))}
      </div>
    </div>
  );
}
