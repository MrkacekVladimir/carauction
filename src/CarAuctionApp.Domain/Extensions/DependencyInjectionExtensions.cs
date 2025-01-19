using CarAuctionApp.Domain.Users.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CarAuctionApp.Domain.Extensions;

public static class DependencyInjectionExtensions
{
    public static void AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<UserService>();
    }
}
