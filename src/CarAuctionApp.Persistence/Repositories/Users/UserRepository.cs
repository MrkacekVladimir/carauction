using CarAuctionApp.Domain.Users.Entities;
using CarAuctionApp.Domain.Users.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CarAuctionApp.Persistence.Repositories.Users;

public class UserRepository : IUserRepository
{
    private readonly AuctionDbContext _dbContext;

    public UserRepository(AuctionDbContext dbContext)
    {
        this._dbContext = dbContext;
    }
    public Task AddAsync(User user)
    {
        _dbContext.Users.Add(user);
        return Task.CompletedTask;
    }

    public Task<bool> IsUsernameAvailableAsync(string username)
    {
        return _dbContext.Users.AnyAsync(u => u.Username == username);
    }
}
