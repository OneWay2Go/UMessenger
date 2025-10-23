using Messenger.Application.DTOs;
using Messenger.Domain.Entities;

namespace Messenger.Application.Interfaces
{
    public interface IMessageRepository : IRepository<Message>
    {
        Task<IEnumerable<Message>> GetByChatIdAsync(int chatId);

        Task UpdateWithUserCheckAsync(UpdateMessageDto dto, int userId);

        Task DeleteWithUserCheckAsync(int messageId, int userId);
    }
}
