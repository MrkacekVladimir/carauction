using CarAuctionApp.Domain.Auctions.DomainEvents;
using CarAuctionApp.Domain.Auctions.Entities;
using CarAuctionApp.Domain.Auctions.ValueObjects;
using CarAuctionApp.Domain.Users.Entities;

namespace CarAuctionApp.Domain.UnitTests.Auctions;

public class AuctionTests
{
    [Fact]
    public void AddBid_ShouldPass()
    {
        // Arrange
        AuctionDate auctionDate = AuctionDate.Create(DateTime.UtcNow, DateTime.UtcNow.AddDays(2)).Value!;
        Auction auction = Auction.Create("Test auction", auctionDate).Value!;
        User user = new User("Test user");
        BidAmount amount = new BidAmount(1000);

        // Act
        auction.AddBid(user, amount);
        
        // Assert
        Assert.Single(auction.Bids);
        var bid = auction.Bids.ToList()[0];
        Assert.Equal(user, bid.User);
        Assert.Equal(amount, bid.Amount);
    }

    [Fact]
    public void Create_ShouldRaiseUserCreatedEvent()
    {
        // Arrange
        string title = "Test auction";
        AuctionDate auctionDate = AuctionDate.Create(DateTime.UtcNow, DateTime.UtcNow.AddDays(2)).Value!;

        // Act
        Auction auction = Auction.Create(title, auctionDate).Value!;
        var events = auction.DomainEvents.ToList();

        // Assert
        Assert.Single(events);
        var createdEvent = events[0];
        Assert.True(createdEvent is AuctionCreatedEvent);
        Assert.Equal(title, (createdEvent as AuctionCreatedEvent)!.Title);
    }

    [Fact]
    public void AddBid_ShouldReturnError_WhenAmountIsLowerThanCurrentMaxBid()
    {
        // Arrange
        AuctionDate auctionDate = AuctionDate.Create(DateTime.UtcNow, DateTime.UtcNow.AddDays(2)).Value!;
        Auction auction = Auction.Create("Test auction", auctionDate).Value!;
        User user = new User("Test user");
        BidAmount amount = new BidAmount(1000);
        auction.AddBid(user, amount);

        // Act
        var result = auction.AddBid(user, new BidAmount(500));

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Null(result.Value);
    }

    [Fact]
    public void AddBid_ShouldReturnError_WhenAmountIsEqualToCurrentMaxBid()
    {
        // Arrange
        AuctionDate auctionDate = AuctionDate.Create(DateTime.UtcNow, DateTime.UtcNow.AddDays(2)).Value!;
        Auction auction = Auction.Create("Test auction", auctionDate).Value!;
        User user = new User("Test user");
        BidAmount amount = new BidAmount(1000);
        auction.AddBid(user, amount);

        // Act
        var result = auction.AddBid(user, amount);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Null(result.Value);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void AddBid_ShouldReturnError_WhenAuctionIsNotOpen(bool future)
    {
        // Arrange
        DateTime start = future ? DateTime.UtcNow.AddHours(5) : DateTime.UtcNow.AddDays(-2);
        DateTime end = future ? DateTime.UtcNow.AddHours(8) : DateTime.UtcNow.AddDays(-1);
        AuctionDate auctionDate = AuctionDate.Create(start, end).Value!;
        Auction auction = Auction.Create("Test auction", auctionDate).Value!;
        User user = new User("Test user");
        BidAmount amount = new BidAmount(1000);
        auction.AddBid(user, amount);

        // Act
        var result = auction.AddBid(user, amount);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Null(result.Value);
    }

}
