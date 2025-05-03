using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KarmaWebAPI.Models
{
    public class Alumne
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [StringLength(10)]
        public String NIA { get; set; } //identificador únic de tamany màxim 10 que assigna gva
        
        [Required]
        public String Nom { get; set; } //nom de l'alumne   
        
        [Required]
        public String Cognoms { get; set; } //cognoms de l'alumne   

        [Required]
        public Boolean Actiu { get; set; } = true; //indica si està actiu o no 
        
        [Required]
        [StringLength(255)]
        public String Email { get; set; } //correu electrònic 

        public ICollection<AlumneEnGrup> AlumneEnGrups { get; set; } //relació amb alumne grups
        //public object AlumnesEnGrup { get; internal set; }
    }
}
