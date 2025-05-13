using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KarmaWebAPI.DTOs
{

    public class PrivilegiAssignatEditarDto
    {
        [Required]
        public int IdPrivilegiAssignat { get; set; } //identificar únic del any escolar

        [Required]
        public DateOnly DataExecucio { get; set; } //identificar únic del privilegi assignat
    }

}
