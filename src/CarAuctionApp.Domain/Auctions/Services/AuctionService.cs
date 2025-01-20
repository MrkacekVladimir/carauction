using CarAuctionApp.Domain.Auctions.Entities;
using CarAuctionApp.Domain.Auctions.Repositories;
using CarAuctionApp.Domain.Auctions.ValueObjects;
using CarAuctionApp.Domain.Shared;

namespace CarAuctionApp.Domain.Auctions.Services;

public interface IAuctionService
{
    Task<Result<Auction?>> CreateAuctionAsync(string title, DateTime startsOn, DateTime endsOn);
}   
public class AuctionService: IAuctionService
{
    private readonly IAuctionRepository _auctionRepository;

    public AuctionService(IAuctionRepository auctionRepository)
    {
        this._auctionRepository = auctionRepository;
    }

    public async Task<Result<Auction?>> CreateAuctionAsync(string title, DateTime startsOn, DateTime endsOn)
    {
        var auctionDateResult = AuctionDate.Create(startsOn, endsOn);
        if (!auctionDateResult.IsSuccess || auctionDateResult.Value == null)
        {
            return Result<Auction?>.Failure(auctionDateResult.Error);
        }

        var auction = new Auction(title, auctionDateResult.Value);

        await _auctionRepository.AddAsync(auction);

        return Result<Auction?>.Success(auction);
    }
}
