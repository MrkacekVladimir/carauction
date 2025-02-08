using CarAuctionApp.Application.Authentication;
using CarAuctionApp.WebApi.Services;

namespace CarAuctionApp.WebApi.Extensions;

internal static class ServicesExtensions
{
    public static void AddApiServices(this IServiceCollection services)
    {
        services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();
    }
}
