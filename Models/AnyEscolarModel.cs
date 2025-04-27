using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KarmaWebAPI.Models
{
    public class AnyEscolar
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdAnyEscolar { get; set; } //identificar únic
        public DateOnly DataIniciCurs { get; set; }
        public DateOnly DataFiCurs { get; set; }
        public Boolean Actiu { get; set; }
        public int DiesPeriode { get; set; } // dies per període

        #region Navegacions
        // Propiedad de navegación para los Privilegis asociados
        public ICollection<Privilegi> Privilegis { get; set; }

        public ICollection<Periode> Periodes{ get; set; }

        public ICollection<Grup> Grups { get; set; }

        public ICollection<ConfiguraKarma> ConfiguracionsKarma { get; set; }
        #endregion Navegacions

    }
}
