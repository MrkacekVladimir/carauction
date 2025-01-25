import { Link } from "react-router";
import { AuctionDto, AuctionListItemDto } from "@/lib/api/types";
import carSvg from "@/assets/car.svg";
import classes from "./AuctionCard.module.css";

interface Props {
  auction: AuctionDto | AuctionListItemDto;
}
export function AuctionCard({ auction }: Props) {
  return (
    <section className={classes.wrapper}>
      <div className={classes.image}>
        <img src={carSvg} alt="Car" />
      </div>
      <div className={classes.details}>
        <div className={classes.title}>{auction.title}</div>
        <div className={classes.description}>
          <div>Starts on: {auction.startsOn.toLocaleString()}</div>
          <div>Ends on: {auction.endsOn.toLocaleString()}</div>
          <div>Bids: {auction.bids.length}</div>
        </div>
        <Link to={auction.id}>To auction</Link>
      </div>
    </section>
  );
}
