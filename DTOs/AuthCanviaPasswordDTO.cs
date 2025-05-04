using System.ComponentModel.DataAnnotations;

namespace KarmaWebAPI.DTOs
{

    public class AuthCanviaPasswordDTO
    {
        [Required]
        public string PasswordActual { get; set; }

        [Required]
        public string NouPassword { get; set; }
    }
}
