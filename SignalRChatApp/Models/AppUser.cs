using Microsoft.AspNetCore.Identity;

namespace SignalRChatApp.Models
{
    public class AppUser : IdentityUser
    {
        public ICollection<RoomUser> Rooms { get; set; }
    }
}
