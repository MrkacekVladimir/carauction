using CarAuctionApp.Application.Authentication;
using CarAuctionApp.Domain.Users.Entities;

namespace CarAuctionApp.Processor.Services;

public class NullCurrentUserProvider : ICurrentUserProvider
{
    public Task<User?> GetCurrentUserAsync()
    {
        return Task.FromResult<User?>(null);
    }
}
