using CarAuctionApp.SharedKernel;
using CarAuctionApp.Domain.Users.Entities;

namespace CarAuctionApp.Domain.Users.Repositories;

public interface IUserRepository
{
    Task<User?> GetById(Guid id);
    Task<bool> IsUsernameAvailableAsync(string username);
    Task<Result<User>> AddAsync(User user);
}
