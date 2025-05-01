using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using Microsoft.EntityFrameworkCore;

namespace KarmaWebAPI.Models
{
    [Keyless]
    public class VPrivilegiPeriode
    {
        [Required]
        public int IdPeriode { get; set; } //identificar únic
        
        [Required]
        public int IdAlumneEnGrup { get; set; }
        
        [Required]
        public int IdPrivilegi { get; set; } //identificar únic


        #region Navegacions
        public AlumneEnGrup AlumneEnGrup { get; set; } //Alumne en grup
        public Privilegi Privilegi { get; set; } //Privilegi
        public Periode Periode { get; set; } //Periode

        #endregion Navegacions
    }
}
