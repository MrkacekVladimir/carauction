import { AuctionBidDto } from "@/lib/api/types";
import { AuctionBidInput } from "@/components/bid-input";
import { useAuctionBiddingBook } from "./useAuctionBiddingBook";

interface Props {
  auctionId: string;
  initialBids?: AuctionBidDto[];
  canBid: boolean;
}

export function AuctionBiddingBook({ auctionId, initialBids, canBid }: Props) {
  const { connected, bids } = useAuctionBiddingBook(
    auctionId,
    initialBids ?? []
  );

  if (!connected) {
    return <div>Connecting to bidding book...</div>;
  }

  return (
    <div>
      <h2>Bidding book</h2>
      {canBid && <AuctionBidInput auctionId={auctionId} />}
      <table>
        <thead>
          <tr>
            <th>Bid Amount</th>
            <th>Bid On</th>
          </tr>
        </thead>
        <tbody>
          {bids.map((b, i) => (
            <tr key={i}>
              <td>{b.amount}</td>
              <td>{b.createdOn.toLocaleString()}</td>
            </tr>
          ))}
        </tbody>
      </table>
      <ul></ul>
    </div>
  );
}
