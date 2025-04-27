using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace KarmaWebAPI.Models
{
    public class VPrivilegiPeriode
    {
        [Required]
        public int IdPeriode { get; set; } //identificar únic
        
        [Required]
        public String IdAlumneGrup { get; set; }
        
        [Required]
        public int IdPrivilegi { get; set; } //identificar únic


        #region Navegacions
        public AlumneEnGrup AlumneEnGrup { get; set; } //Alumne en grup
        public Privilegi Privilegi { get; set; } //Privilegi
        public Periode Periode { get; set; } //Periode

        #endregion Navegacions
    }
}
