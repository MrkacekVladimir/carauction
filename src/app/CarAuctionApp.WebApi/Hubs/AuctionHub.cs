using Microsoft.AspNetCore.SignalR;

namespace CarAuctionApp.WebApi.Hubs;

public class AuctionHub : Hub<IAuctionHubClient>
{
    public async Task JoinAuctionGroup(Guid auctionId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, auctionId.ToString());
    }

    public async Task LeaveAuctionGroup(Guid auctionId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, auctionId.ToString());
    }
}

