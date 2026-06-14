using System.ComponentModel.DataAnnotations;

namespace KarmaWebAPI.DTOs
{
    public class AssignarPrivilegiDTO
    {
        [Required]
        public string NIA { get; set; } = string.Empty;

        [Required]
        public long IdPrivilegi { get; set; }
    }

}
