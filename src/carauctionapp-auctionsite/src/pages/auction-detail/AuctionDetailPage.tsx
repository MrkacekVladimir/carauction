import { Link, useParams } from "react-router";
import { AuctionBiddingBook } from "../../components/bidding-book/AuctionBiddingBook";
import { useAuctionQuery } from "./useAuctionQuery";

export function AuctionDetailPage() {
  const { auctionId } = useParams();
  const { data, isPending, isError } = useAuctionQuery(auctionId!);

  if (isError || auctionId === undefined) {
    return <div>Failed to fetch auction...</div>;
  }

  if (isPending) {
    return <div>Loading auction...</div>;
  }

  const auction = data.auction;

  return (
    <div>
      <Link to={"/auctions"}>Back to auctions</Link>
      <h1>{auction.title}</h1>
      <ul>
        <li>Starts On: {auction.startsOn.toLocaleString()}</li>
        <li>Ends On: {auction.endsOn.toLocaleString()}</li>
      </ul>
      <AuctionBiddingBook auctionId={auctionId} initialBids={auction.bids} />
    </div>
  );
}
