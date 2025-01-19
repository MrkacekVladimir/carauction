using CarAuctionApp.Domain.Auctions.Entities;
using CarAuctionApp.Domain.Auctions.Repositories;

namespace CarAuctionApp.Domain.Auctions.Services
{
    public class AuctionService
    {
        private readonly IAuctionRepository _auctionRepository;

        public AuctionService(IAuctionRepository auctionRepository)
        {
            this._auctionRepository = auctionRepository;
        }

        public async Task<Auction> CreateAuctionAsync(string title)
        {
            var auction = new Auction(title);

            _auctionRepository.Add(auction);

            return auction;
        }
    }
}
