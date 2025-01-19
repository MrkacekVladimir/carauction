namespace CarAuctionApp.Domain;

public abstract class EntityBase
{
    public DateTime CreatedOn { get; private set; } = DateTime.UtcNow;
    public DateTime? LastUpdatedOn { get; set; }
}
