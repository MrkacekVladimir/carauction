import { apiUrl } from "@/lib/api/config";
import { CreateBidRequest } from "@/lib/api/types";
import { useRef } from "react";

interface Props {
  auctionId: string;
}

export function AuctionBidInput({ auctionId }: Props) {
  const inputRef = useRef<HTMLInputElement | null>(null);

  const sendBid = async () => {
    if (inputRef.current) {
      const value = inputRef.current.valueAsNumber;
      const model: CreateBidRequest = {
        amount: value,
      };

      const response = await fetch(`${apiUrl}/auctions/${auctionId}/bids`, {
        headers: {
          "Content-Type": "application/json",
        },
        method: "POST",
        body: JSON.stringify(model),
      });

      console.log(response.status);
    }
  };

  return (
    <div>
      <label>Amount</label>
      <input ref={inputRef} name="amount" type="number" />
      <button onClick={sendBid}>Send bid</button>
    </div>
  );
}
