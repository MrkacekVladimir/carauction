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
        User owner = new User("Owner");
        AuctionDate auctionDate = AuctionDate.Create(DateTime.UtcNow, DateTime.UtcNow.AddDays(2)).Value!;
        Auction auction = Auction.Create(owner, "Test auction", auctionDate).Value!;
        User bidder = new User("Bidder");
        BidAmount amount = new BidAmount(1000);

        // Act
        auction.AddBid(bidder, amount);
        
        // Assert
        Assert.Single(auction.Bids);
        var bid = auction.Bids.ToList()[0];
        Assert.Equal(bidder, bid.User);
        Assert.Equal(amount, bid.Amount);
    }

    [Fact]
    public void Create_ShouldRaiseUserCreatedEvent()
    {
        // Arrange
        User owner = new User("Owner");
        string title = "Test auction";
        AuctionDate auctionDate = AuctionDate.Create(DateTime.UtcNow, DateTime.UtcNow.AddDays(2)).Value!;

        // Act
        Auction auction = Auction.Create(owner, title, auctionDate).Value!;
        var events = auction.DomainEvents.ToList();

        // Assert
        Assert.Single(events);
        var createdEvent = events[0];
        Assert.True(createdEvent is AuctionCreatedDomainEvent);
        Assert.Equal(title, (createdEvent as AuctionCreatedDomainEvent)!.Title);
    }

    [Fact]
    public void AddBid_ShouldReturnError_WhenAmountIsLowerThanCurrentMaxBid()
    {
        // Arrange
        User owner = new User("Owner");
        AuctionDate auctionDate = AuctionDate.Create(DateTime.UtcNow, DateTime.UtcNow.AddDays(2)).Value!;
        Auction auction = Auction.Create(owner, "Test auction", auctionDate).Value!;
        User bidder = new User("Bidder");
        BidAmount amount = new BidAmount(1000);
        auction.AddBid(bidder, amount);

        // Act
        var result = auction.AddBid(bidder, new BidAmount(500));

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Null(result.Value);
    }

    [Fact]
    public void AddBid_ShouldReturnError_WhenAmountIsEqualToCurrentMaxBid()
    {
        // Arrange
        User owner = new User("Owner");
        AuctionDate auctionDate = AuctionDate.Create(DateTime.UtcNow, DateTime.UtcNow.AddDays(2)).Value!;
        Auction auction = Auction.Create(owner, "Test auction", auctionDate).Value!;
        User bidder = new User("Bidder");
        BidAmount amount = new BidAmount(1000);
        auction.AddBid(bidder, amount);

        // Act
        var result = auction.AddBid(bidder, amount);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Null(result.Value);
    }

    [Fact]
    public void AddBid_ShouldReturnError_WhenBidderIsTheSameAsOwner()
    {
        // Arrange
        User owner = new User("Owner");
        AuctionDate auctionDate = AuctionDate.Create(DateTime.UtcNow, DateTime.UtcNow.AddDays(2)).Value!;
        Auction auction = Auction.Create(owner, "Test auction", auctionDate).Value!;
        BidAmount amount = new BidAmount(1000);
        auction.AddBid(owner, amount);

        // Act
        var result = auction.AddBid(owner, amount);

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
        User owner = new User("Owner");
        DateTime start = future ? DateTime.UtcNow.AddHours(5) : DateTime.UtcNow.AddDays(-2);
        DateTime end = future ? DateTime.UtcNow.AddHours(8) : DateTime.UtcNow.AddDays(-1);
        AuctionDate auctionDate = AuctionDate.Create(start, end).Value!;
        Auction auction = Auction.Create(owner, "Test auction", auctionDate).Value!;
        User bidder = new User("Bidder");
        BidAmount amount = new BidAmount(1000);
        auction.AddBid(bidder, amount);

        // Act
        var result = auction.AddBid(bidder, amount);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Null(result.Value);
    }

}
