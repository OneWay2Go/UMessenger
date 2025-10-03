using Messenger.Domain.Entities;

namespace Messenger.Application.Interfaces
{
    public interface IAuthService
    {
        string CreateJwt(User user);
        string GenerateRefreshToken();
    }
}
