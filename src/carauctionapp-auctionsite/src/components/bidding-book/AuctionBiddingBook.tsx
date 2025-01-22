import { AuctionBidDto } from "@/lib/api/types";
import { useAuctionBiddingBook } from "./useAuctionBiddingBook";
import { AuctionBidInput } from "../bid-input/AuctionBidInput";

interface Props {
  auctionId: string;
  initialBids?: AuctionBidDto[];
}

export function AuctionBiddingBook({ auctionId, initialBids }: Props) {
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
      <AuctionBidInput auctionId={auctionId} />
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
