using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace KarmaWebAPI.DTOs
{

    public class AlumneDTO
    {
        [Required]
        public string NIA { get; set; }

        [Required]
        public string Nom { get; set; } //nom de l'alumne 
        
        [Required]
        public string Cognoms { get; set; } //cognoms de l'alumne 

        [Required] 
        public string Email { get; set; } //correu electrònic 
    }

}
