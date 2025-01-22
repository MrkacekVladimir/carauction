export type AuctionListItemDto = {
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
