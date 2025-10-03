namespace Messenger.Domain.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }

        public string Token { get; set; }

        public DateTime ExpiresOnUtc { get; set; }

        public int UserId { get; set; }

        public User User {get; set;}
    }
}
