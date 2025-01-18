using CarAuctionApp.Domain.Entities;
using CarAuctionApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CarAuctionApp.WebApi.Extensions;

public static class PersistenceExtensions
{
    public static async Task ApplyMigrationsAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AuctionDbContext>();
        await dbContext.Database.MigrateAsync();
    }

    public static async Task SeedDevelopmentData(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AuctionDbContext>();
        if (!dbContext.Users.Any())
        {
            dbContext.Users.Add(new User("DevTest"));
            await dbContext.SaveChangesAsync();
        }
    }

}
