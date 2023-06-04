namespace SignalRChatApp.Models
{
    public class BannedUser
    {
        public string UserId { get; set; }

        public AppUser User { get; set; }

        public int RoomId { get; set; }

        public Room Room { get; set; }
    }
}
