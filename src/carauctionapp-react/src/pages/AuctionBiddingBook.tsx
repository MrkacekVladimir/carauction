import { useConfig } from "@/hooks/useConfig";
import { HubConnectionBuilder } from "@microsoft/signalr";
import { useEffect, useState } from "react";

function useAuctionBiddingBook(auctionId: string) {
  const config = useConfig();
  const [connected, setConnected] = useState(false);
  const [bids, setBids] = useState<number[]>([]);

  useEffect(() => {
    let connection = new HubConnectionBuilder()
      .withUrl(`${config.apiUrl}/hubs/auction`)
      .build();

    connection.on("ReceiveBidUpdate", (_: string, bidAmount: number) => {
      setBids((prevBids) => [...prevBids, bidAmount]);
    });

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

interface Props {
  auctionId: string;
}

export function AuctionBiddingBook({ auctionId }: Props) {
  const { connected, bids } = useAuctionBiddingBook(auctionId);

  if (!connected) {
    return <div>Connecting to bidding book...</div>;
  }

  return (
    <div>
      <h2>Bidding book</h2>
      <ul>
        {bids.map((b, i) => (
          <li key={i}>{b}</li>
        ))}
      </ul>
    </div>
  );
}
