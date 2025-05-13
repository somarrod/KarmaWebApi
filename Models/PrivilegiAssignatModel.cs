using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace KarmaWebAPI.Models
{
    public class PrivilegiAssignat

    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdPrivilegiAssignat { get; set; } //identificar únic

        [ForeignKey("Privilegi")]
        public int IdPrivilegi { get; set; } //identificar únic del privilegi assignat

        [ForeignKey("AlumneEnGrup")]
        public int IdAlumneEnGrup { get; set; } //identificar únic del privilegi assignat

        [Required]
        public int Nivell { get; set; }

        [Required]
        public String Descripcio { get; set; }

        [Required]
        public String EsIndividualGrup { get; set; } //valors possibles: 'I' o 'G'

        [Required]
        public DateOnly DataAssignacio { get; set; } //data assignació

        public DateOnly? DataExecucio { get; set; } = null; //data en el que 


        #region Navegacions

        public Privilegi Privilegi { get; set; } //Privilegi
        public AlumneEnGrup AlumneEnGrup { get; set; } //Alumne en grup

        #endregion Navegacions
    }
}