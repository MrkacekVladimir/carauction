export type AuctionListItemDto = {
  id: string;
  title: string;
  startsOn: Date;
  endsOn: Date;
  bids: AuctionBidDto[];
};

export type AuctionBidUserDto = {
  id: string;
  username: string;
};

export type AuctionBidDto = {
  id: string;
  amount: number;
  createdOn: Date;
  user: AuctionBidUserDto;
};
export type AuctionDto = {
  id: string;
  title: string;
  startsOn: Date;
  endsOn: Date;
  bids: AuctionBidDto[];
};

export type CreateAuctionRequest = {
  title: string;
  startsOn: Date;
  endsOn: Date;
};

export type CreateBidRequest = {
  amount: number;
};

export type GetAuctionResponse = {
  auction: AuctionDto;
};

export type GetAuctionsResponse = {
  auctions: AuctionListItemDto[];
};
