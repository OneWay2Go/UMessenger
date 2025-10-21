using Messenger.Domain.Entities;

namespace Messenger.Application.Interfaces
{
    public interface IMessageRepository : IRepository<Message>
    {
        Task<IEnumerable<Message>> GetByChatIdAsync(int chatId);
    }
}
