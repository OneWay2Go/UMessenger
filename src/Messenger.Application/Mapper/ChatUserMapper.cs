using Messenger.Application.DTOs;
using Messenger.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Messenger.Application.Mapper
{
    [Mapper]
    public partial class ChatUserMapper
    {
        public partial ChatUser AddChatUserDtoToChatUser(AddChatUserDto dto);
    }
}
