using Messenger.Application.Interfaces;
using Messenger.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Messenger.Infrastructure.Persistence.Repositories
{
    public class RefreshTokenRepository(
        MessengerDbContext context,
        IHttpContextAccessor httpContextAccessor) : Repository<RefreshToken>(context), IRefreshTokenRepository
    {
        public async Task<bool> DeleteRefreshTokensByUserId(int userId)
        {
            if (userId != GetCurrentUserId())
                throw new ApplicationException("You can't do this.");

            await context.RefreshTokens.Where(r => r.UserId == userId).ExecuteDeleteAsync();

            return true;
        }

        // Fix this shit
        private int? GetCurrentUserId()
        {
            var claim = httpContextAccessor.HttpContext?.User.FindFirst("sub");
            if (claim is null || claim.Value is null)
                throw new ApplicationException("claim or claim's value not found.");

            var parsed = int.Parse(claim.Value);

            return parsed;
        }

        public async Task<RefreshToken> GetByRefreshToken(string refreshToken)
        {
            RefreshToken? RefreshToken = await context.RefreshTokens
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Token == refreshToken);

            if (RefreshToken is null || RefreshToken.ExpiresOnUtc < DateTime.UtcNow)
                throw new ApplicationException("Refresh token is expired.");

            return RefreshToken;
        }
    }
}
