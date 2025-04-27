namespace KarmaWebAPI.DTOs
{

    public class AnyEscolarCrearDto
    {
        public DateOnly DataIniciCurs { get; set; }
        public DateOnly DataFiCurs { get; set; }
        public int DiesPeriode { get; set; } // dies per període
    }

}
