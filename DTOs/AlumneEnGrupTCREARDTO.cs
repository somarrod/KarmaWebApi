using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace KarmaWebAPI.DTOs
{

    public class AlumneEnGrupTCREARDTO
    {
        [Required]
        [StringLength(10)] 
        public string NIA { get; set; }

        [Required]
        public int IdAnyEscolar { get; set; } //identificar únic del grup

        [Required]
        [StringLength(50)]
        public String IdGrup { get; set; } //identificar únic del grup

    }

}
