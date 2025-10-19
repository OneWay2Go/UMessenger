namespace Messenger.Application.DTOs
{
    public class AddMessageDto
    {
        public string Content { get; set; } = default!;  // Text OR file description

        // File attachment fields
        public string? FileUrl { get; set; }      // Stored file path/URL
        public string? FileName { get; set; }     // Original filename
        public long? FileSize { get; set; }       // Size in bytes
        public string? FileType { get; set; }     // mime-type (image/jpeg, application/pdf)
        public bool IsAttachment { get; set; } = false;

        public int SenderId { get; set; }
        public int ChatId { get; set; }
    }
}
