using CarAuctionApp.Application.Authentication;
using CarAuctionApp.Domain.Users.Entities;
using CarAuctionApp.Domain.Users.Repositories;
using System.Security.Claims;

namespace CarAuctionApp.WebApi.Services
{
    public class CurrentUserProvider : ICurrentUserProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;

        public CurrentUserProvider(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
        {
            this._httpContextAccessor = httpContextAccessor;
            this._userRepository = userRepository;
        }

        public async Task<User?> GetCurrentUserAsync()
        {
            var principal = _httpContextAccessor.HttpContext?.User;
            if(principal == null)
            {
                return null;
            }

            var id = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            if(id == null)
            {
                return null;
            }

            var user = await _userRepository.GetById(Guid.Parse(id));

            return user;
        }
    }
}
