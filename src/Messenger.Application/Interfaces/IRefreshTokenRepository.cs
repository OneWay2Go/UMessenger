using Messenger.Domain.Entities;

namespace Messenger.Application.Interfaces
{
    public interface IRefreshTokenRepository : IRepository<RefreshToken>
    {
        Task<RefreshToken> GetByRefreshToken(string refreshToken);
        Task<bool> DeleteRefreshTokensByUserId(int userId);
    }
}
