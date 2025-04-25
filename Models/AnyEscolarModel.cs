using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KarmaWebAPI.Models
{
    public class AnyEscolar
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdAnyEscolar { get; set; } //identificar únic
        public DateTime DataIniciCurs { get; set; }
        public DateTime DataFiCurs { get; set; }
        public Boolean Actiu { get; set; }
        public int DiesPeriode { get; set; } // dies per període


        // Propiedad de navegación para los Privilegis asociados
        public ICollection<Privilegi> Privilegis { get; set; }

    }
}
