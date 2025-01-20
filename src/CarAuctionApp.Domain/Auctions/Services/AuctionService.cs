using CarAuctionApp.Domain.Auctions.Entities;
using CarAuctionApp.Domain.Auctions.Repositories;
using CarAuctionApp.Domain.Auctions.ValueObjects;

namespace CarAuctionApp.Domain.Auctions.Services;

public interface IAuctionService
{
    Task<Auction> CreateAuctionAsync(string title, DateTime startsOn, DateTime endsOn);
}   
public class AuctionService: IAuctionService
{
    private readonly IAuctionRepository _auctionRepository;

    public AuctionService(IAuctionRepository auctionRepository)
    {
        this._auctionRepository = auctionRepository;
    }

    public Task<Auction> CreateAuctionAsync(string title, DateTime startsOn, DateTime endsOn)
    {
        var auctionDate = new AuctionDate(startsOn, endsOn);
        var auction = new Auction(title, auctionDate);

        _auctionRepository.AddAsync(auction);

        return Task.FromResult(auction);
    }
}
