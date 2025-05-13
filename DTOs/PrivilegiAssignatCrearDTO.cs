using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KarmaWebAPI.DTOs
{

    public class PrivilegiAssignatCrearDto
    {
        [Required]
        public int IdAnyEscolar { get; set; } //identificar únic del any escolar

        [Required]
        [StringLength(50)]
        public String IdGrup { get; set; }

        [Required]
        [StringLength(10)]
        public String NIA { get; set; }

        [Required]
        public int IdPrivilegi { get; set; } //identificar únic del privilegi assignat
    }

}
