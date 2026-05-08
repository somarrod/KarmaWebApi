using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace KarmaWebAPI.DTOs
{

    public class AlumneDTO
    {
        [Required]
        [StringLength(10)] 
        public string NIA { get; set; }

        [Required]
        public string Nom { get; set; } //nom de l'alumne 
        
        [Required]
        public string Cognoms { get; set; } //cognoms de l'alumne 

        [Required] 
        public string Email { get; set; } //correu electrònic 

        public long? IdClasse { get; set; } //classe --> opcional

        public long? IdGrup { get; set; } //opcional i ha de coincidir en la classe
    }

}
