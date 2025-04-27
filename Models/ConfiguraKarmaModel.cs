using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KarmaWebAPI.Models
{
    public class ConfiguraKarma
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdConfiguraKarma { get; set; } //identificador únic

        [Required]
        [ForeignKey("AnyEscolar")]
        public int IdAnyEscolar { get; set; } //identificador de l'any escolar
        
        [Required]
        public int KarmaMinim { get; set; } //Karma mínim per a l'usuari
        
        [Required]
        public int KarmaMaxim { get; set; } //Karma màxim per a l'usuari   

        [Required]
        public String ColorNivell { get; set; }

        public int NivellPrivilegis { get; set; } //Nivell de privilegis per a l'usuari

        #region Navegacions
        // Propiedad de navegación para referenciar a AnyEscolar
        public AnyEscolar AnyEscolar { get; set; } = null!;
        #endregion Navegacions
    }
}
