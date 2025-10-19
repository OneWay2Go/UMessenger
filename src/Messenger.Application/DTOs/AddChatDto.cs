using Messenger.Domain.Enums;

namespace Messenger.Application.DTOs
{
    public class AddChatDto
    {
        public string Name { get; set; }

        public ChatType ChatType { get; set; }
    }
}
