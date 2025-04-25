namespace KarmaWebAPI.DTOs
{

    public class AnyEscolarCrearDto
    {
        public DateTime DataIniciCurs { get; set; }
        public DateTime DataFiCurs { get; set; }
        public Boolean Actiu { get; set; }
        public int DiesPeriode { get; set; } // dies per període
    }

}
