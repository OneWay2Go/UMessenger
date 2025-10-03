using Messenger.Application.Interfaces;
using Messenger.Domain.Entities;

namespace Messenger.Infrastructure.Persistence.Repositories
{
    public class MessageRepository(MessengerDbContext context) : Repository<Message>(context), IMessageRepository
    {
    }
}
