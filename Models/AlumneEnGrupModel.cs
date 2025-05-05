using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace KarmaWebAPI.Models
{
    public class AlumneEnGrup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdAlumneEnGrup { get; set; } //identificar únic

        [ForeignKey("Alumne")]
        [Required]
        [StringLength(10)]
        public String NIA { get; set; } //identificar únic de tamany màxim 10 que assigna gva


        [Required]
        [ForeignKey("AnyEscolar")]
        public int IdAnyEscolar { get; set; } //identificar únic del grup

        [Required]
        [ForeignKey("Grup")]
        [StringLength(50)]
        public String IdGrup { get; set; } //identificar únic del grup

        public int PuntuacioTotal { get; set; } = 0; //Puntuacio total de l'alumne en el grup i any escolar

        [StringLength(10)]
        public String Karma { get; set; } = "CREACIÓ"; //Color de Karma assignat a l'alumne


        #region Navegacion
        public Alumne Alumne { get; set; }
        public Grup Grup { get; set; } //identificar únic del grup
        public AnyEscolar AnyEscolar { get; set; } //identificar únic del grup
        public ICollection<Puntuacio> Puntuacions{ get; set; } //Puntuacions de l'alumne assignades

        [NotMapped]
        public ICollection<VPrivilegiPeriode> PrivilegisPeriode { get; set; } //Puntuacions de l'alumne assignades

        #endregion Navegacion
    }
}
