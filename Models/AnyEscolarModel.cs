using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KarmaWebAPI.Models
{
    public class AnyEscolar
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdAnyEscolar { get; set; } //identificar únic

        [Required]
        public DateOnly DataIniciCurs { get; set; }

        [Required]
        public DateOnly DataFiCurs { get; set; }


        /// <summary>
        /// Saldo inicial de karma que tindrà cada alumne en iniciar el curs
        /// </summary>
        [Required]
        public double SaldoKarmaInicial { get; set; }

        /// <summary>
        /// Indica si el saldo de karma es reinicia en cada avaluació
        /// </summary>
        [Required]
        public bool ReiniciaCadaAvaluacio { get; set; }

        /// <summary>
        /// Indica si l'any escolar està actiu
        /// </summary>
        [Required]
        public bool Actiu { get; set; } = true;




        #region Navegacions

        public ICollection<Privilegi> Privilegis { get; set; } = new List<Privilegi>();
        public ICollection<Avaluacio> Avaluacios { get; set; } = new List<Avaluacio>();
        public ICollection<Grup> Grups { get; set; } = new List<Grup>();
        public ICollection<ConfiguracioKarma> ConfiguracionsKarma { get; set; } = new List<ConfiguracioKarma>();

        #endregion

    }
}
