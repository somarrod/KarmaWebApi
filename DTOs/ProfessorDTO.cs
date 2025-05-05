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
        [StringLength(200)]
        public string Nom { get; set; } //nom de l'alumne 
        
        [Required]
        [StringLength(200)]
        public string Cognoms { get; set; } //cognoms de l'alumne 

        [Required]
        [StringLength(255)]
        public string Email { get; set; } //correu electrònic 
    }

}
