using CarAuctionApp.Application.DomainEvents;
using CarAuctionApp.SharedKernel.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace CarAuctionApp.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(config => config.RegisterServicesFromAssembly(ApplicationAssemblyReference.Assembly));
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
    }

}
