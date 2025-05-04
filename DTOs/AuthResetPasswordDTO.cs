using System.ComponentModel.DataAnnotations;

namespace KarmaWebAPI.DTOs
{
    public class AuthResetPasswordDTO
    {

        [Required]
        public string Email { get; set; }

        [Required]
        public string NouPassword { get; set; }

    }
}
