using Messenger.Domain.Enums;

namespace Messenger.Domain.Entities
{
    public class ChatUser
    {
        public int Id { get; set; }
        public int ChatId { get; set; }
        public int UserId { get; set; }
        public ChatRole Role { get; set; } = ChatRole.Member;
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;

        // Navigation
        public Chat Chat { get; set; } = default!;
        public User User { get; set; } = default!;
    }
}
