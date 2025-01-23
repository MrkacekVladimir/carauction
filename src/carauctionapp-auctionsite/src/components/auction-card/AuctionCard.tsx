import { Link } from "react-router";
import { AuctionDto, AuctionListItemDto } from "@/lib/api/types";
import carSvg from "@/assets/car.svg";
import classes from "./AuctionCard.module.css";

interface Props {
  auction: AuctionDto | AuctionListItemDto;
}
export function AuctionCard({ auction }: Props) {
  return (
    <section className={classes.card}>
      <div className={classes.image}>
        <img src={carSvg} alt="Car" />
      </div>
      <div className={classes.details}>
        <div>{auction.title}</div>
        <div>Bids: {auction.bids.length}</div>
        <Link to={auction.id}>To auction</Link>
      </div>
    </section>
  );
}
