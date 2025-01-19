using CarAuctionApp.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CarAuctionApp.Persistence.Extensions;

public static class PersistenceExtensions
{
    public static void AddPersistence(this IServiceCollection services, string connectionString)
    {
        services.AddDbContextPool<AuctionDbContext>((sp, options) =>
        {
            options.AddInterceptors(new DomainEventToOutboxMessagesInterceptor());
            options.UseNpgsql(connectionString);
        });
    }

}
