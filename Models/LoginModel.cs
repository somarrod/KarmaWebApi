using System.ComponentModel.DataAnnotations;

namespace KarmaWebAPI.Models
{

    public class LoginModel
    {
        [Required]      
        [StringLength(255)]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Display(Name = "Em recordes")]
        public bool RememberMe { get; set; }
    }

}
