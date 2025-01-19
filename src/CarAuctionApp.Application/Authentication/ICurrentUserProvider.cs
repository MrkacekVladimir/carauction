using CarAuctionApp.Domain.Users.Entities;

namespace CarAuctionApp.Application.Authentication;

public interface ICurrentUserProvider
{
    public Task<User?> GetCurrentUserAsync();
}
