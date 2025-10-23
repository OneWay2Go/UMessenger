using Messenger.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Messenger.Domain.Entities
{
    public class Message
    {
        public int Id { get; set; }

        [Required]
        public string Content { get; set; } = default!;  // Text OR file description

        // File attachment fields
        public string? FileUrl { get; set; }      // Stored file path/URL
        public string? FileName { get; set; }     // Original filename
        public long? FileSize { get; set; }       // Size in bytes
        public string? FileType { get; set; }     // mime-type (image/jpeg, application/pdf)
        public bool IsAttachment { get; set; } = false;

        public bool IsDeleted { get; set; } = false;

        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public MessageStatus Status { get; set; } = MessageStatus.Delivered;

        public int UserId { get; set; }
        public User User { get; set; } = default!;

        public int ChatId { get; set; }
        public Chat Chat { get; set; } = default!;
    }
}
