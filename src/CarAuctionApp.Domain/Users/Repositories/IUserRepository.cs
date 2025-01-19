using CarAuctionApp.Domain.Users.Entities;

namespace CarAuctionApp.Domain.Users.Repositories;

public interface IUserRepository
{
    Task<bool> IsUsernameAvailableAsync(string username);
    Task AddAsync(User user);
}
