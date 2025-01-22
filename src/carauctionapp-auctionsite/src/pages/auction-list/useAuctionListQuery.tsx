import { apiUrl } from "@/lib/api/config";
import { AuctionListItemDto } from "@/lib/api/types";
import { useQuery } from "@tanstack/react-query";

export function useAuctionListQuery() {
  return useQuery({
    queryKey: ["auctions"],
    queryFn: async () => {
      const response = await fetch(`${apiUrl}/auctions`);
      const jsonData = await response.json();
      return jsonData as AuctionListItemDto[];
    },
  });
}
