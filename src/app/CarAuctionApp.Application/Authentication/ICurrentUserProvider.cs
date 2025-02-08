using CarAuctionApp.Domain.Users.Entities;
using System.Threading.Tasks;

namespace CarAuctionApp.Application.Authentication;

public interface ICurrentUserProvider
{
    public Task<User?> GetCurrentUserAsync();
}
