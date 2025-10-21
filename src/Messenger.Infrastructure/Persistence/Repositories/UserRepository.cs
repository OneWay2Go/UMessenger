using Messenger.Application.Interfaces;
using Messenger.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Infrastructure.Persistence.Repositories
{
    public class UserRepository(MessengerDbContext context) : Repository<User>(context), IUserRepository
    {
        public async Task<User?> GetByEmailAsync(string email)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                return null;
            return user;
        }

        public async Task<IEnumerable<User>> SearchUsersAsync(string query)
        {
            return await context.Users
                .Where(u => u.Email.Contains(query) || u.Username.Contains(query))
                .ToListAsync();
        }
    }
}
