using Messenger.Application.DTOs;
using Messenger.Application.Interfaces;
using Messenger.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Infrastructure.Persistence.Repositories
{
    public class MessageRepository(MessengerDbContext context) : Repository<Message>(context), IMessageRepository
    {
        public async Task DeleteWithUserCheckAsync(int messageId, int userId)
        {
            var message = await context.Messages
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.Id == messageId);

            if (message == null)
                throw new Exception("There is no message with this id.");

            if (message.UserId != userId)
                throw new Exception("User do not own this message to delete.");

            message.IsDeleted = true;

            context.Messages.Update(message);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Message>> GetByChatIdAsync(int chatId)
        {
            return await context.Set<Message>()
                .Where(m => m.ChatId == chatId && m.IsDeleted == false)
                .OrderBy(m => m.SentAt)
                .ToListAsync();
        }

        public async Task UpdateWithUserCheckAsync(UpdateMessageDto dto, int userId)
        {
            var message = await context.Messages
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.Id == dto.Id);

            if (message == null)
                throw new Exception("There is no message with this id.");

            if (message.UserId != userId)
                throw new Exception("User do not own this message to change.");

            message.Content = dto.Content;

            context.Messages.Update(message);
            await context.SaveChangesAsync();
        }
    }
}
