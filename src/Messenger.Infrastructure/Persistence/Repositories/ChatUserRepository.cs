using Messenger.Application.Interfaces;
using Messenger.Domain.Entities;

namespace Messenger.Infrastructure.Persistence.Repositories
{
    public class ChatUserRepository(MessengerDbContext context) : Repository<ChatUser>(context), IChatUserRepository
    {
    }
}
