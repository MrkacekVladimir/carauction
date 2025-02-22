using CarAuctionApp.Domain.Auctions.Repositories;
using CarAuctionApp.Domain.Users.Repositories;
using CarAuctionApp.Persistence.Interceptors;
using CarAuctionApp.Persistence.Outbox;
using CarAuctionApp.Persistence.Repositories.Auctions;
using CarAuctionApp.Persistence.Repositories.Users;
using CarAuctionApp.SharedKernel;
using CarAuctionApp.SharedKernel.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CarAuctionApp.Persistence.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddPersistence(this IServiceCollection services, string connectionString)
    {
        services.AddScoped<IAuctionRepository, AuctionRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IIntegrationEventPublisher, IntegrationEventPublisher>();
        services.AddTransient<DomainEventInterceptor>();

        services.AddDbContext<AuctionDbContext>((sp, options) =>
        {
            var outboxInterceptor = sp.GetRequiredService<DomainEventInterceptor>();
            options.AddInterceptors(outboxInterceptor);
            options.UseNpgsql(connectionString);
        });
    }

}
