using System.ComponentModel.DataAnnotations;

namespace KarmaWebAPI.Models
{

    public class LoginModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Display(Name = "Em recordes")]
        public bool RememberMe { get; set; }
    }

}
