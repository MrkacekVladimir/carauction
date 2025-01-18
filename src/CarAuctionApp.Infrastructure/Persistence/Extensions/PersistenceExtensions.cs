using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CarAuctionApp.Infrastructure.Persistence.Extensions;

public static class PersistenceExtensions
{
    public static void AddPersistence(this IServiceCollection services, string connectionString)
    {
        services.AddDbContextPool<AuctionDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });
    }

}
