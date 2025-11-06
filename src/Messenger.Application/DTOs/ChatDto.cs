using Messenger.Domain.Enums;
using Messenger.Domain.Entities;

namespace Messenger.Application.DTOs
{
    public class ChatDto
    {
        public string Name { get; set; } = default!;
        public ChatType Type { get; set; } = ChatType.Private;
        public string? ChatImageUrl { get; set; }  // Group avatar
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string CurrentUserRole { get; set; }

        public List<ChatUser> ChatUsers { get; set; }
    }
}
