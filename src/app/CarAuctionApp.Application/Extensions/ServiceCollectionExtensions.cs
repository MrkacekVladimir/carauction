using CarAuctionApp.SharedKernel.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace CarAuctionApp.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
    }

}
