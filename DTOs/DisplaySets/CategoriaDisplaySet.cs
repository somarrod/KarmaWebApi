namespace KarmaWebAPI.DTOs.DisplaySets
{
    public class CategoriaDisplaySet
    {

        public long IdCategoria { get; set; }
        public string Descripcio { get; set; }
        public double NumPunts { get; set; }
        public bool Editable { get; set; }
        public string? Comentaris { get; set; }
        public bool Activa { get; set; }

        public long IdTipusCategoria { get; set; }
        public string DescripcioTipusCategoria { get; set; }

    }
}
