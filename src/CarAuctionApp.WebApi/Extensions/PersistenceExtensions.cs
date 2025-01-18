using CarAuctionApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CarAuctionApp.WebApi.Extensions;

public static class PersisteneExtensions
{
    public static async Task ApplyMigrationsAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AuctionDbContext>();
        await dbContext.Database.MigrateAsync();
    }

}
