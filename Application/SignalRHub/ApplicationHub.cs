using Microsoft.AspNetCore.SignalR;

namespace Application.SignalRHub
{
    public class ApplicationHub : Hub<IApplicationHubClient>
    {
    }
}
