using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace KarmaWebAPI.Models
{
    [Keyless]
    public class VPrivilegiPeriode
    {
        public int IdPeriode { get; set; } //identificar únic
        public int IdAlumneEnGrup { get; set; }
        public int IdPrivilegi { get; set; } //identificar únic


        #region Navegacions
        public AlumneEnGrup AlumneEnGrup { get; set; } //Alumne en grup
        public Privilegi Privilegi { get; set; } //Privilegi
        public Periode Periode { get; set; } //Periode

        #endregion Navegacions
    }
}
