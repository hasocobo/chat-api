    namespace ChatAPI.Core.Entities;
    public class Message
    {
        public Guid Id { get; set; }
        public required string SenderUsername { get; set; }
        public string? ReceiverUsername { get; set; }
        public required string Content { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

}