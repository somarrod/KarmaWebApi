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
        public String NIA { get; set; } //identificar únic de tamany màxim 10 que assigna gva
      
        [Required]
        [ForeignKey("Grup")]
        public String IdGrup { get; set; } //identificar únic del grup


        #region Navegacion
        public Alumne Alumne { get; set; }
        public Grup Grup { get; set; } //identificar únic del grup

        public List<Puntuacio> Puntuacions{ get; set; } //Puntuacions de l'alumne assignades

        //public List<PrivilegiPeriode> PrivilegisPeriode { get; set; } //Puntuacions de l'alumne assignades

        #endregion Navegacion
    }
}
