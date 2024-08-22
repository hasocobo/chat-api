namespace ChatAPI.Application.DTOs.Message;


    public class MessageReadDto
    {
        public int Id { get; set; }
        public required string SenderUsername { get; set; }
        public string? ReceiverUsername { get; set; }
        public required string Content { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }