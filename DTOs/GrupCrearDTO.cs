using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace KarmaWebAPI.DTOs
{

    public class GrupCrearDTO
    {

        [Required]
        public long IdClasse { get; set; } //identificar únic

        [Required]
        [StringLength(20)]  
        public string Nom{ get; set; }
    }

}
