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

    public async Task<User> CreateUserAsync(string username)
    {
        if(!await _userRepository.IsUsernameAvailableAsync(username))
        {
            throw new Exception("Username is not available");
        }

        var user = new User(username);

        await _userRepository.AddAsync(user);

        return user;
    }
}
