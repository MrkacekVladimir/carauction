import { apiUrl } from "@/lib/api/config";
import { CreateAuctionRequest } from "@/lib/api/types";
import { useMutation } from "@tanstack/react-query";

export function useCreateAuctionMutation() {
  return useMutation({
    mutationFn: async (model: CreateAuctionRequest) => {
      return await fetch(`${apiUrl}/auctions`, {
        headers: {
          "Content-Type": "application/json",
        },
        method: "POST",
        body: JSON.stringify(model),
      });
    },
  });
}
