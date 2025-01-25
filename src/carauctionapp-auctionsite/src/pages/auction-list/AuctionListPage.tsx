import { AuctionCard } from "@/components/auction-card";
import { AuctionFilter } from "@/components/auction-filter";
import { useAuctionListQuery } from "./useAuctionListQuery";
import classes from "./AuctionListPage.module.css";

export function AuctionListPage() {
  const { data, isPending, isError } = useAuctionListQuery();

  if (isError) {
    return <div>Failed to load auction list...</div>;
  }

  if (isPending) {
    return <div>Loading auctions...</div>;
  }

  const auctions = data.auctions;

  return (
    <div>
      <h1>Auctions</h1>
      <AuctionFilter />
      <section className={classes.auctionList}>
        {auctions.map((a) => (
          <AuctionCard key={a.id} auction={a} />
        ))}
      </section>
    </div>
  );
}
