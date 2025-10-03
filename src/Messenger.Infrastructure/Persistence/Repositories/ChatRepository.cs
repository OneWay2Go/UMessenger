using Messenger.Application.Interfaces;
using Messenger.Domain.Entities;

namespace Messenger.Infrastructure.Persistence.Repositories
{
    public class ChatRepository(MessengerDbContext context) : Repository<Chat>(context), IChatRepository
    {
    }
}
