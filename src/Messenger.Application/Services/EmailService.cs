using Messenger.Application.DTOs;
using Messenger.Application.Interfaces;
using Messenger.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Net.Mail;

namespace Messenger.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(
            IOptions<EmailSettings> emailSettings,
            IUserRepository userRepository)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailCodeAsync(User user)
        {
            using var smtpClient = new SmtpClient(_emailSettings.Host, _emailSettings.Port);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;

            smtpClient.Credentials = new NetworkCredential(_emailSettings.Email, _emailSettings.Password);

            var subject = "UMessenger email confirmation code.";
            var body = $"Hello, Dear {user.Email}!\n\nYour confirmation code is: {user.EmailConfirmationCode}.";
            var message = new MailMessage(_emailSettings.Email!, user.Email, subject, body);
            await smtpClient.SendMailAsync(message);
        }

        public async Task<bool> VerifyEmailCodeAsync(User user, int inputCode)
        {
            if (user.EmailConfirmationCode == null)
                return false;

            if (inputCode != user.EmailConfirmationCode)
                return false;

            return true;
        }
    }
}
