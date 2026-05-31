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
        
       
        public long? IdClasse { get; set; }
        [ForeignKey(nameof(IdClasse))]
        public Classe? Classe { get; set; } 


        // Grup (pot canviar)
        
        public long? IdGrup { get; set; }
        [ForeignKey(nameof(IdGrup))]
        public Grup? Grup { get; set; }

        public double? KarmaActualPunts;
        [StringLength(20)]
        public string? KarmaActualColor;
    }
}
