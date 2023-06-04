namespace SignalRChatApp.Models
{
    public class Room
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public ICollection<Message> Messages { get; set; }

        public ICollection<RoomUser> Users { get; set; }

        public string Type { get; set; }
    }
}
