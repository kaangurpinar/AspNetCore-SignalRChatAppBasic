namespace SignalRChatApp.Models
{
    public class Message
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Text { get; set; }

        public DateTime TimeStamp { get; set; }

        public int RoomId { get; set; }

        public Room Room { get; set; }
    }
}
