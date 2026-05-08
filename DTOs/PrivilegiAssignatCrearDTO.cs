using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KarmaWebAPI.DTOs
{

    public class PrivilegiAssignatCrearDto
    {
        [Required]
        public int IdAnyEscolar { get; set; } //identificar únic del any escolar

        [Required]
        public long IdClasse { get; set; }

        [Required]
        [StringLength(10)]
        public string NIA { get; set; }

        [Required]
        public int IdPrivilegi { get; set; } //identificar únic del privilegi assignat
    }

}
