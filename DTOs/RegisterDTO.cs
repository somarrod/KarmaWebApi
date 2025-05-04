using System.ComponentModel.DataAnnotations;

namespace KarmaWebAPI.DTOs
{

    public class RegisterDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        //[Required]
        //[RoleValidation]
        //public string Role { get; set; }
    }

}
