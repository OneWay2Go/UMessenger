using Messenger.Domain.Entities;

namespace Messenger.Application.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        public Task<User?> GetByEmailAsync(string email);
        public Task<IEnumerable<User>> SearchUsersAsync(string query);
    }
}
