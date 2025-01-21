using CarAuctionApp.SharedKernel;
using CarAuctionApp.Domain.Auctions.Repositories;
using CarAuctionApp.Domain.Users.Repositories;
using CarAuctionApp.Persistence.Interceptors;
using CarAuctionApp.Persistence.Repositories.Auctions;
using CarAuctionApp.Persistence.Repositories.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CarAuctionApp.Persistence.Extensions;

public static class PersistenceExtensions
{
    public static void AddPersistence(this IServiceCollection services, string connectionString)
    {
        services.AddScoped<IAuctionRepository, AuctionRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork>((sp) => sp.GetRequiredService<AuctionDbContext>());
        services.AddSingleton<DomainEventToOutboxMessagesInterceptor>();

        services.AddDbContext<AuctionDbContext>((sp, options) =>
        {
            var outboxInterceptor = sp.GetRequiredService<DomainEventToOutboxMessagesInterceptor>();
            options.AddInterceptors(outboxInterceptor);
            options.UseNpgsql(connectionString);
        });
    }

}
