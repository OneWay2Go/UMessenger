namespace Messenger.Application.DTOs
{
    public class AddRefreshTokenDto
    {
        public string Token { get; set; }

        public DateTime ExpiresOnUtc { get; set; }

        public int UserId { get; set; }
    }
}
