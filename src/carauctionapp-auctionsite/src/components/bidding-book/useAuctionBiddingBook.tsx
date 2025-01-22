import { useConfig } from "@/hooks/useConfig";
import { AuctionBidDto } from "@/lib/api/types";
import { HubConnectionBuilder } from "@microsoft/signalr";
import { useState, useEffect } from "react";

type BiddingBookBid = {
  id: string;
  amount: number;
  createdOn: Date;
};

export function useAuctionBiddingBook(
  auctionId: string,
  initialBids: AuctionBidDto[]
) {
  const config = useConfig();
  const [connected, setConnected] = useState(false);
  const [bids, setBids] = useState<BiddingBookBid[]>(
    initialBids.map((b) => ({
      id: b.id,
      amount: b.amount,
      createdOn: b.createdOn,
    }))
  );

  useEffect(() => {
    let connection = new HubConnectionBuilder()
      .withUrl(`${config.apiUrl}/hubs/auction`)
      .build();

    connection.on(
      "ReceiveBidUpdate",
      (_: string, bidId: string, bidAmount: number, createdOn: string) => {
        const newBid: BiddingBookBid = {
          id: bidId,
          amount: bidAmount,
          createdOn: new Date(createdOn),
        };
        setBids((prevBids) => [newBid, ...prevBids]);
      }
    );

    async function connect() {
      await connection.start();
      await connection.invoke("JoinAuctionGroup", auctionId);
      setConnected(true);
    }

    connect();

    return () => {
      connection.stop();
    };
  }, []);

  return { connected, bids };
}
