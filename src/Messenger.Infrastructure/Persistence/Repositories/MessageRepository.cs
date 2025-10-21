using Messenger.Application.Interfaces;
using Messenger.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Infrastructure.Persistence.Repositories
{
    public class MessageRepository(MessengerDbContext context) : Repository<Message>(context), IMessageRepository
    {
        public async Task<IEnumerable<Message>> GetByChatIdAsync(int chatId)
        {
            return await context.Set<Message>()
                .Where(m => m.ChatId == chatId)
                .OrderBy(m => m.SentAt)
                .ToListAsync();
        }
    }
}
