using CarAuctionApp.Domain.Users.Entities;

namespace CarAuctionApp.Domain
{
    public interface IDomainUserProvider
    {
        public Task<User?> GetUserAsync();
    }
}
