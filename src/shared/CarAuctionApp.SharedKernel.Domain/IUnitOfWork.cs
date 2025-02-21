namespace CarAuctionApp.SharedKernel.Domain;


public interface IUnitOfWork
{
    public Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
