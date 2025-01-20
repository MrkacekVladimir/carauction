namespace CarAuctionApp.Domain.Auctions.ValueObjects;

public record AuctionDate
{
    public AuctionDate(DateTime startsOn, DateTime endsOn)
    {
        if (endsOn < startsOn)
        {
            throw new Exception("End date cannot be earlier than start date.");
        }

        StartsOn = startsOn;
        EndsOn = endsOn;
    }

    public DateTime StartsOn { get; private set; }
    public DateTime EndsOn { get; private set; }
    public DateTime? LastBidOn { get; private set; }
    public bool HasEnded => DateTime.UtcNow > EndsOn;
    public bool IsRunning => DateTime.UtcNow >= StartsOn && DateTime.UtcNow <= EndsOn;

    public void ExtendEndsOn(TimeSpan time)
    {
        EndsOn.Add(time);
    }

    public void UpdateLastBidOn(DateTime lastBidOn)
    {
        if (lastBidOn >= EndsOn || lastBidOn <= StartsOn)
        {
            throw new Exception("End date cannot be earlier than start date or in the future.");
        }

        LastBidOn = lastBidOn;
    }
}
