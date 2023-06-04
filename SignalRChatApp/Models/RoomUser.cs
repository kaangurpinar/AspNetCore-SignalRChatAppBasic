namespace SignalRChatApp.Models
{
    public class RoomUser
    {
        public string UserId { get; set; }

        public AppUser User { get; set; }

        public int RoomId { get; set; }

        public Room Room { get; set; }

        //public UserRole Role { get; set; }
    }
}
