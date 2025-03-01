using CarAuctionApp.Domain.Users.Entities;
using CarAuctionApp.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CarAuctionApp.WebApi.Extensions;

internal static class WebApplicationExtensions
{
    internal static async Task ApplyMigrationsAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AuctionDbContext>();
        await dbContext.Database.MigrateAsync();
    }

    internal static async Task SeedDevelopmentData(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AuctionDbContext>();
        if (!dbContext.Users.Any())
        {
            dbContext.Users.Add(new User("DevTest"));
            await dbContext.SaveChangesAsync();
        }
    }

    internal static void UseExceptionHandlerWithFallback(this WebApplication app)
    {
        app.UseExceptionHandler(exceptionHandlerApp =>
        {
            exceptionHandlerApp.Run(async httpContext =>
            {
                var pds = httpContext.RequestServices.GetService<IProblemDetailsService>();
                if (pds == null
                    || !await pds.TryWriteAsync(new() { HttpContext = httpContext }))
                {
                    // Fallback behavior
                    await httpContext.Response.WriteAsync("Fallback: An error occurred.");
                }
            });
        });
    }
}
