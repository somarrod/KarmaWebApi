using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KarmaWebAPI.DTOs
{

    public class PrivilegiEditarDTO
    {
        [Required]
        public long IdPrivilegi { get; set; }


        [Required]
        public string Descripcio { get; set; }

        [Required]
        public string Tipus { get; set; }

        // Depén del curs escolar
        [Required]
        public long IdAnyEscolar { get; set; }

        // nivell mínim de karma necessari
        [Required]
        public int NivellPrivilegi { get; set; }

        public bool Actiu { get; set; } = true;
    }

}
