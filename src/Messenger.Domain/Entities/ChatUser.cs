using Messenger.Domain.Enums;

namespace Messenger.Domain.Entities
{
    public class ChatUser
    {
        public int Id { get; set; }
        public int ChatId { get; set; }
        public int UserId { get; set; }
        public ChatRole Role { get; set; } = ChatRole.Member;  // ← Uses the new enum!
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public Chat Chat { get; set; } = default!;
        public User User { get; set; } = default!;
    }
}
