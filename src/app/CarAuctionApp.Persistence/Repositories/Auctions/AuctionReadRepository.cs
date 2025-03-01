using CarAuctionApp.Application.Features.Auctions.Dtos;
using CarAuctionApp.Application.Features.Auctions.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarAuctionApp.Persistence.Repositories.Auctions;

public class AuctionReadRepository(AuctionDbContext dbContext) : IAuctionReadRepository
{
    public async Task<AuctionDto?> GetAuctionByIdAsync(Guid auctionId, CancellationToken cancellationToken)
    {
        var data = await dbContext.Auctions.AsNoTracking()
                .Where(a => a.Id == auctionId)
                .Select(a => new
                {
                    a.Id,
                    a.Title,
                    StartsOn = a.Date.StartsOn,
                    EndsOn = a.Date.EndsOn,
                    Bids = a.Bids.OrderByDescending(b => b.Amount.Value)
                    .Select(b => new
                    {
                        b.Id,
                        Amount = b.Amount.Value,
                        b.CreatedOn,
                        User = new { b.User.Id, b.User.Username }
                    })
                })
                .FirstOrDefaultAsync();

        if (data is null)
        {
            return null;
        }

        var auctionDto = new AuctionDto(
            data.Id,
            data.Title,
            data.StartsOn,
            data.EndsOn,
            data.Bids.Select(b => new AuctionBidDto(
                b.Id,
                b.Amount,
                b.CreatedOn,
                new AuctionBidUserDto(b.User.Id, b.User.Username)
            ))
        );

        return auctionDto;
    }

    public async Task<IEnumerable<AuctionListItemDto>> GetAuctionsListAsync(CancellationToken cancellationToken)
    {
        var auctions = await dbContext.Auctions.AsNoTracking()
        .Select(a =>
        new AuctionListItemDto(
            a.Id,
            a.Title,
            a.Date.StartsOn,
            a.Date.EndsOn,
            a.Bids.Select(b => new AuctionBidDto(
                b.Id,
                b.Amount.Value,
                b.CreatedOn,
                new AuctionBidUserDto(b.User.Id, b.User.Username))
            )
        )).ToListAsync(cancellationToken);

        return auctions;
    }

    public async Task<IEnumerable<AuctionListItemDto>> SearchListByFullTextAsync(string search, CancellationToken cancellationToken)
    {
        var auctions = await dbContext.Auctions.AsNoTracking()
        .Where(a => EF.Functions.ToTsVector("english", a.Title + " " + a.Description)
            .Matches(EF.Functions.PhraseToTsQuery("english", search)))
        .Select(a =>
        new AuctionListItemDto(
            a.Id,
            a.Title,
            a.Date.StartsOn,
            a.Date.EndsOn,
            a.Bids.Select(b => new AuctionBidDto(
                b.Id,
                b.Amount.Value,
                b.CreatedOn,
                new AuctionBidUserDto(b.User.Id, b.User.Username))
            )
        )).ToListAsync(cancellationToken);

        return auctions;
    }
}
