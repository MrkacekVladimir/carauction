namespace CarAuctionApp.Application.Authentication;

public interface ICurrentUserProvider
{
    public async Task<User?> GetCurrentUserAsync();
}
