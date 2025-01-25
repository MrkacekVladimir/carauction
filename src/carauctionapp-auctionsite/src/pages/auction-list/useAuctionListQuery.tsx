import { apiUrl } from "@/lib/api/config";
import { GetAuctionsResponse } from "@/lib/api/types";
import { jsonReviver } from "@/utils/json";
import { useQuery } from "@tanstack/react-query";

export function useAuctionListQuery() {
  return useQuery({
    queryKey: ["auctions"],
    queryFn: async () => {
      const response = await fetch(`${apiUrl}/auctions`);
      const textData = await response.text();
      return JSON.parse(textData, jsonReviver) as GetAuctionsResponse;
    },
  });
}
