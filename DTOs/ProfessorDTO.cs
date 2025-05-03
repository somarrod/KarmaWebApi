using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace KarmaWebAPI.DTOs
{

    public class ProfessorDTO
    {
        [Required]
        [StringLength(50)]
        public String IdProfessor { get; set; }

        [Required]
        public String Nom { get; set; } //nom de l'alumne 
        
        [Required]
        public String Cognoms { get; set; } //cognoms de l'alumne 

        [Required] 
        public string Email { get; set; } //correu electrònic 
    }

}
