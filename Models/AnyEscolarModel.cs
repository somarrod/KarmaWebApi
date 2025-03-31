using System.ComponentModel.DataAnnotations;

namespace KarmaWebAPI.Models
{
    public class AnyEscolar
    {
        [Key]
        public int id_anyEscolar { get; set; } //identificar únic
        public DateTime dataIniciCurs { get; set; }
        public DateTime dataFiCurs { get; set; }
        public Boolean actiu { get; set; } 


    }
}
