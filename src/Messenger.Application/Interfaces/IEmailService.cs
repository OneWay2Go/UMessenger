using Messenger.Domain.Entities;

namespace Messenger.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailCodeAsync(User user);

        Task<bool> VerifyEmailCodeAsync(User user, int inputCode);
    }
}
