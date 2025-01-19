using CarAuctionApp.Domain.Auctions.ValueObjects;

namespace CarAuctionApp.Domain.UnitTests.Auctions;

public class BidAmountTests
{
    [Fact]
    public void Constructor_ShouldPass()
    {
        // Arrange
        decimal amount = 1000;

        // Act
        BidAmount bidAmount = new BidAmount(1000);

        // Assert
        Assert.Equal(amount, bidAmount.Value);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1000)]
    public void Constructor_ShouldThrow_WhenAmountIsIncorrect(decimal amount)
    {
        // Arrange/Act/Assert
        Assert.Throws<ArgumentException>(() => new BidAmount(amount));
    }
}
