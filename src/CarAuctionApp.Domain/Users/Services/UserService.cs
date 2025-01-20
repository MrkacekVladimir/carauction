using CarAuctionApp.Domain.Shared;
using CarAuctionApp.Domain.Users.Entities;
using CarAuctionApp.Domain.Users.Repositories;

namespace CarAuctionApp.Domain.Users.Services;

public class UserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        this._userRepository = userRepository;
    }

    public async Task<Result<User?>> CreateUserAsync(string username)
    {
        if(!await _userRepository.IsUsernameAvailableAsync(username))
        {
            return Result<User?>.Failure(new Error("USER001", "Username is already taken"));
        }

        var user = new User(username);

        await _userRepository.AddAsync(user);

        return Result<User?>.Success(user);
    }
}
