using Messenger.Domain.Entities;

namespace Messenger.Application.DTOs
{
    public class GlobalSearchResponseDto
    {
        public IList<User> Users { get; set; }

        public IList<Chat> Chats { get; set; }

        public IList<Chat> UserExistingChats { get; set; }
    }
}
