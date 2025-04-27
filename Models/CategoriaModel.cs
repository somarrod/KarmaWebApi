using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KarmaWebAPI.Models
{
    public class Categoria
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdCategoria { get; set; } //identificar únic
        public String Descripcio { get; set; } //nom 
        public bool Activa { get; set; } = true; //Activa, per defecte true
    }
}
