using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KarmaWebAPI.Models
{
    public class Alumne
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [StringLength(10)]
        public string NIA { get; set; } //identificador únic de tamany màxim 10 que assigna gva
        
        [Required]
        [StringLength(200)]
        public string Nom { get; set; } //nom de l'alumne   
        
        [Required]
        [StringLength(200)]
        public string Cognoms { get; set; } //cognoms de l'alumne   

        [Required]
        public Boolean Actiu { get; set; } = true; //indica si està actiu o no 
        
        [Required]
        [StringLength(255)]
        public string Email { get; set; } //correu electrònic 


        [Required]
        [ForeignKey(nameof(Classe))]
        public long? IdClasse { get; set; }
        public Classe? Classe { get; set; } = null!;


        // Grup (pot canviar)
        [Required]
        [ForeignKey(nameof(Grup))]
        public long? IdGrup { get; set; }
        public Grup? Grup { get; set; } = null!;


        public double? KarmaActualPunts;
        public string? KarmaActualColor;
    }
}
