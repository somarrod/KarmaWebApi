using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KarmaWebAPI.Models
{
    public class Grup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IdGrup { get; set; }

        [StringLength(20)]
        public string Nom { get; set; } = string.Empty;


        
        [Required]
        public long IdClasse { get; set; }
        [ForeignKey(nameof(IdClasse))]
        public Classe Classe { get; set; } = null!;

        // DERIVAT / CACHE
        [StringLength(20)]
        public string? KarmaBase { get; set; }
        public DateTime? DataUltimaActualitzacioKarma { get; set; }
    }
}
