using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace KarmaWebAPI.DTOs
{


    public class CategoriaEditarDTO
    {
        public long IdCategoria { get; set; }
        public string Descripcio { get; set; } = string.Empty;
        public double NumPunts { get; set; }
        public bool Editable { get; set; }
        public string? Comentaris { get; set; }
        public bool Activa { get; set; }
        public long IdTipusCategoria { get; set; }
    }


}
