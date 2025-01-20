using CarAuctionApp.Domain.Shared;
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
    public Task<Result<User>> AddAsync(User user)
    {
        _dbContext.Users.Add(user);
        return Task.FromResult(Result<User>.Success(user));
    }

    public async Task<User?> GetById(Guid id)
    {
        //TODO: Remove fakes user
        //return _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
        return await _dbContext.Users.FirstAsync();
    }

    public Task<bool> IsUsernameAvailableAsync(string username)
    {
        return _dbContext.Users.AnyAsync(u => u.Username == username);
    }
}
