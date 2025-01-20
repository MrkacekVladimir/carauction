import { apiUrl } from "@/lib/api/config";
import { useQuery } from "@tanstack/react-query";
import { Link, NavLink } from "react-router";

type AuctionListItemDto = {
  id: string;
  title: string;
  bids: {
    id: string;
    amount: string;
    user: {
      id: string;
      username: string;
    };
  }[];
};

export function AuctionListPage() {
  const {
    data: auctions,
    isPending,
    isError,
  } = useQuery({
    queryKey: ["auctions"],
    queryFn: async () => {
      const response = await fetch(`${apiUrl}/auctions`);
      const jsonData = await response.json();
      return jsonData as AuctionListItemDto[];
    },
  });

  if (isError) {
    return <div>Failed to load auction list...</div>;
  }

  if (isPending) {
    return <div>Loading auctions...</div>;
  }

  return (
    <div>
      <h1>Auction List Page</h1>
      <div>
        {auctions.map((a) => (
          <section>
            <div key={a.id}>{a.title}</div>
            <div>Bids: {a.bids.length}</div>
            <Link to={a.id}>To auction</Link>
          </section>
        ))}
      </div>
    </div>
  );
}
