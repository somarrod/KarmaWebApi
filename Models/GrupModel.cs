using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KarmaWebAPI.Models
{
    public class Grup
    {

        public long IdGrup { get; set; }
        [StringLength(20)]
        public string Nom { get; set; } = string.Empty;

        [Required]
        public long IdClasse { get; set; }
        public Classe Classe { get; set; } = null!;

        // DERIVAT / CACHE
        [StringLength(20)]
        public string? KarmaBase { get; set; }
        public DateTime? DataUltimaActualitzacioKarma { get; set; }
    }
}
