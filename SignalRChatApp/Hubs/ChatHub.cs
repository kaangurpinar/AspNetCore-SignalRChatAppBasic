using Microsoft.AspNetCore.SignalR;

namespace SignalRChatApp.Hubs
{
    public class ChatHub : Hub
    {
        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }
    }
}
