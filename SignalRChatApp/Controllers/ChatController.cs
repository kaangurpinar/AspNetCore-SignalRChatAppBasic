using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalRChatApp.Hubs;
using SignalRChatApp.Models;
using SignalRChatApp.Repository;

namespace SignalRChatApp.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class ChatController : Controller
    {
        private readonly IHubContext<ChatHub> _hubContext;

        private readonly AppDbContext _appDbContext;

        public ChatController(IHubContext<ChatHub> hubContext, AppDbContext appDbContext)
        {
            _hubContext = hubContext;
            _appDbContext = appDbContext;
        }

        [HttpPost("[action]/{connectionId}/{roomId}")]
        public async Task<IActionResult> JoinRoom(string connectionId, string roomId)
        {
            await _hubContext.Groups.AddToGroupAsync(connectionId, roomId);

            return Ok();
        }

        [HttpPost("[action]/{connectionId}/{roomName}")]
        public async Task<IActionResult> LeaveRoom(string connectionId, string roomName)
        {
            await _hubContext.Groups.RemoveFromGroupAsync(connectionId, roomName);

            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SendMessage(int roomId, string message)
        {
            var _message = new Message()
            {
                RoomId = roomId,
                Text = message,
                UserName = User.Identity.Name,
                TimeStamp = DateTime.UtcNow
            };

            _appDbContext.Add(_message);
            await _appDbContext.SaveChangesAsync();

            await _hubContext.Clients.Group(roomId.ToString()).SendAsync("ReceiveMessage", _message);

            return Ok();
        }
    }
}
