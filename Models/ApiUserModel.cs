using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace KarmaWebAPI.Models
{
    public class ApiUser : IdentityUser
    {
        [Required]
        public string login { get; set; }
    }
}
