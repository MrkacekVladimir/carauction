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
        Auction auction = new Auction("Test auction", auctionDate);
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
    public void Constructor_ShouldRaiseUserCreatedEvent()
    {
        // Arrange
        string title = "Test auction";
        AuctionDate auctionDate = AuctionDate.Create(DateTime.UtcNow, DateTime.UtcNow.AddDays(2)).Value!;

        // Act
        Auction auction = new Auction(title, auctionDate);
        var events = auction.DomainEvents.ToList();

        // Assert
        Assert.Single(events);
        var createdEvent = events[0];
        Assert.True(createdEvent is AuctionCreatedEvent);
        Assert.Equal(title, (createdEvent as AuctionCreatedEvent)!.Title);
    }

    [Fact]
    public void AddBid_ShouldThrow_WhenAmountIsLowerThanCurrentMaxBid()
    {
        // Arrange
        AuctionDate auctionDate = AuctionDate.Create(DateTime.UtcNow, DateTime.UtcNow.AddDays(2)).Value!;
        Auction auction = new Auction("Test auction", auctionDate);
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
    public void AddBid_ShouldThrow_WhenAmountIsEqualToCurrentMaxBid()
    {
        // Arrange
        AuctionDate auctionDate = AuctionDate.Create(DateTime.UtcNow, DateTime.UtcNow.AddDays(2)).Value!;
        Auction auction = new Auction("Test auction", auctionDate);
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
