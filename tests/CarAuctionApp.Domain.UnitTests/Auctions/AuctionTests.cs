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
        Auction auction = new Auction("Test auction");
        User user = new User("Test user");
        BidAmount amount = new BidAmount(1000);

        // Act
        AuctionBid bid = auction.AddBid(user, amount);
        
        // Assert
        Assert.Single(auction.Bids);
        Assert.Equal(user, bid.User);
        Assert.Equal(amount, bid.Amount);
    }

    [Fact]
    public void Constructor_ShouldRaiseUserCreatedEvent()
    {
        // Arrange
        string title = "Test auction";

        // Act
        Auction auction = new Auction(title);
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
        Auction auction = new Auction("Test auction");
        User user = new User("Test user");
        BidAmount amount = new BidAmount(1000);
        auction.AddBid(user, amount);

        // Act/Assert
        Assert.Throws<Exception>(() => auction.AddBid(user, new BidAmount(500)));
    }

    [Fact]
    public void AddBid_ShouldThrow_WhenAmountIsEqualToCurrentMaxBid()
    {
        // Arrange
        Auction auction = new Auction("Test auction");
        User user = new User("Test user");
        BidAmount amount = new BidAmount(1000);
        auction.AddBid(user, amount);

        // Act/Assert
        Assert.Throws<Exception>(() => auction.AddBid(user, amount));
    }

}
