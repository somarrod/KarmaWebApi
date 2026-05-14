namespace KarmaWebAPI.DTOs.DisplaySets
{
    public class AnyEscolarDisplaySet
    {
        public int IdAnyEscolar { get; set; } //identificar únic. Es solicita al crear

        public DateOnly DataIniciCurs { get; set; }
        public DateOnly DataFiCurs { get; set; }

        public double SaldoKarmaInicial { get; set; }
        public bool ReiniciaCadaAvaluacio { get; set; }
        public bool Actiu { get; set; }
    }
}
