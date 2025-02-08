import { apiUrl } from "@/lib/api/config";
import { GetAuctionResponse } from "@/lib/api/types";
import { jsonReviver } from "@/utils/json";
import { useQuery } from "@tanstack/react-query";

export function useAuctionQuery(auctionId: string) {
  return useQuery({
    queryKey: ["auctions", auctionId],
    queryFn: async () => {
      const response = await fetch(`${apiUrl}/auctions/${auctionId}`);
      const text = await response.text();
      return JSON.parse(text, jsonReviver) as GetAuctionResponse;
    },
  });
}
