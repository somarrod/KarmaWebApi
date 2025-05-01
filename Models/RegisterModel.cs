using System.ComponentModel.DataAnnotations;

namespace KarmaWebAPI.Models
{

    public class RegisterModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [Required]
        [RoleValidation]
        public string Role { get; set; }
    }

}
