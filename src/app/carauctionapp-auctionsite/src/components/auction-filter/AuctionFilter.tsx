import { useSearchParams } from "react-router";
import classes from "./AuctionFilter.module.css";

export function AuctionFilter() {
  const [search, setSearch] = useSearchParams();
  console.log(search, setSearch);
  //TODO: filters will be persises to URL search params

  return (
    <div className={classes.filter}>
      <div>
        <label>Name</label>
        <input type="text" />
      </div>
      <div>
        <label>In Progress</label>
        <input type="checkbox" />
      </div>
    </div>
  );
}
