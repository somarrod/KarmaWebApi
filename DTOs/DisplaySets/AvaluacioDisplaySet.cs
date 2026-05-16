namespace KarmaWebAPI.DTOs.DisplaySets
{
    public class AvaluacioDisplaySet
    {
        public int IdAnyEscolar { get; set; }
        public long IdAvaluacio { get; set; }
        public string Nom { get; set; } = string.Empty;
        public DateOnly DataInicial { get; set; }
        public DateOnly DataFinal { get; set; }
        public double NotaMinimaKarma { get; set; }
        public double NotaMaximaKarma { get; set; }
       
    }
}
