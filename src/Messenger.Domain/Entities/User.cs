using Messenger.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Messenger.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string? Username { get; set; } = default!;

        public string? DisplayName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; } = default!;

        public bool IsEmailConfirmed { get; set; } = false;

        [Required, MinLength(8)]
        public string Password { get; set; } = default!;

        public Role Role { get; set; } = Role.Member;  // Global app role

        // Profile stuff
        public string? Bio { get; set; }
        public string? ProfileImageUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastSeen { get; set; }

        // Navigation
        public ICollection<Message> Messages { get; set; } = new List<Message>();
        public ICollection<ChatUser> ChatUsers { get; set; } = new List<ChatUser>();
    }
}
