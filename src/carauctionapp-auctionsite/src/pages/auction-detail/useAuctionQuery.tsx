import { apiUrl } from "@/lib/api/config";
import { useQuery } from "@tanstack/react-query";

export function useAuctionQuery(auctionId: string) {
  return useQuery({
    queryKey: ["auctions", auctionId],
    queryFn: async () => {
      const response = await fetch(`${apiUrl}/auctions/${auctionId}`);
      return await response.json();
    },
  });
}
