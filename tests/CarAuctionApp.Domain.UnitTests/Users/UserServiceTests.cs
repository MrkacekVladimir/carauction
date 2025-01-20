using Bogus;
using NSubstitute;
using CarAuctionApp.Domain.Users.Entities;
using CarAuctionApp.Domain.Users.Repositories;
using CarAuctionApp.Domain.Users.Services;

namespace CarAuctionApp.Domain.UnitTests.Users;

public class UserServiceTests
{
    private readonly IUserRepository _userRepository;
    private readonly UserService _userService;
    public UserServiceTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _userService = new UserService(_userRepository);
    }

    [Fact]
    public async Task CreateUserAsync_ShouldPass()
    {
        //Arrange
        var faker = new Faker();
        string fakeUsername = faker.Internet.UserName();
        _userRepository.IsUsernameAvailableAsync(fakeUsername).Returns(true);

        //Act
        var userResult = await _userService.CreateUserAsync(fakeUsername);

        //Assert
        Assert.True(userResult.IsSuccess);
        var user = userResult.Value!;
        Assert.Equal(fakeUsername, user.Username);
        await _userRepository.Received().IsUsernameAvailableAsync(fakeUsername);
        await _userRepository.Received().AddAsync(user);
    }

    [Fact]
    public async Task CreateUserAsync_ShouldThrow_WhenUsernameIsAlreadyTaken()
    {
        //Arrange
        var faker = new Faker();
        string fakeUsername = faker.Internet.UserName();
        _userRepository.IsUsernameAvailableAsync(fakeUsername).Returns(false);

        //Act
        var userResult = await _userService.CreateUserAsync(fakeUsername);

        //Assert
        Assert.False(userResult.IsSuccess);
        await _userRepository.Received().IsUsernameAvailableAsync(fakeUsername);
        await _userRepository.DidNotReceive().AddAsync(Arg.Any<User>());
    }
}
