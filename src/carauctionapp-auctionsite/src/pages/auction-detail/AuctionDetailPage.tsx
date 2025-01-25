import { Link, useParams } from "react-router";
import { AuctionBiddingBook } from "@/components/bidding-book";
import { CountdownTimer } from "@/components/countdown-timer";
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
  const today = new Date();
  const isUpcoming = today < auction.startsOn;
  const canBid = auction.startsOn >= today && auction.endsOn <= today;

  return (
    <div>
      <Link to={"/auctions"}>Back to auctions</Link>
      <h1>{auction.title}</h1>
      <div>
        <div>
          <b>Starts On: </b>
          {auction.startsOn.toLocaleString()}
        </div>
        <div>
          <b>Ends On: </b>
          {auction.endsOn.toLocaleString()}
        </div>
      </div>
      {isUpcoming && <CountdownTimer targetDate={auction.startsOn} />}

      {!isUpcoming && (
        <AuctionBiddingBook
          auctionId={auctionId}
          initialBids={auction.bids}
          canBid={canBid}
        />
      )}
    </div>
  );
}
