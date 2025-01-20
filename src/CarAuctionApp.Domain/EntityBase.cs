namespace CarAuctionApp.Domain;

public abstract class EntityBase
{
    public DateTime CreatedOn { get; init; } = DateTime.UtcNow;
    public DateTime? LastUpdatedOn { get; set; }
}
