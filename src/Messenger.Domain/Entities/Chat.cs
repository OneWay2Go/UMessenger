using Messenger.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Messenger.Domain.Entities
{
    public class Chat
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; } = default!;

        public ChatType Type { get; set; } = ChatType.Private;
        public string? ChatImageUrl { get; set; }  // Group avatar
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public ICollection<Message> Messages { get; set; } = new List<Message>();
        public ICollection<ChatUser> ChatUsers { get; set; } = new List<ChatUser>();
    }
}
