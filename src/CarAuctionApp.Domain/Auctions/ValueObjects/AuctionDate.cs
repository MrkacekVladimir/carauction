using CarAuctionApp.Domain.Shared;

namespace CarAuctionApp.Domain.Auctions.ValueObjects;

public record AuctionDate
{
    private AuctionDate() { }
    private AuctionDate(DateTime startsOn, DateTime endsOn)
    {
        StartsOn = startsOn;
        EndsOn = endsOn;
    }

    public static Result<AuctionDate?> Create(DateTime startsOn, DateTime endsOn)
    {
        if (endsOn < startsOn)
        {
            return Result<AuctionDate?>.Failure(new Error("AuctionDateCreate", "End date cannot be earlier than start date."));
        }

        AuctionDate date = new(startsOn, endsOn);
        return Result<AuctionDate?>.Success(date);
    }

    public DateTime StartsOn { get; private set; }
    public DateTime EndsOn { get; private set; }
    public DateTime? LastBidOn { get; private set; }
    public bool HasEnded => DateTime.UtcNow > EndsOn;
    public bool IsRunning => DateTime.UtcNow >= StartsOn && DateTime.UtcNow <= EndsOn;

    public Result ExtendEndsOn(TimeSpan time)
    {
        EndsOn.Add(time);
        return Result.Success();
    }

    public Result UpdateLastBidOn(DateTime lastBidOn)
    {
        if (lastBidOn >= EndsOn || lastBidOn <= StartsOn)
        {
            return Result.Failure(new Error("AuctionDateUpdateLastBidOn", "End date cannot be earlier than start date or in the future."));
        }

        LastBidOn = lastBidOn;
        return Result.Success();
    }
}
