using System.ComponentModel.DataAnnotations;

namespace Messenger.Application.DTOs
{
    public class AddUserDTO
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(8)]
        public string Password { get; set; }
    }
}
