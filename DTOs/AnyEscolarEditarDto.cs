namespace KarmaWebAPI.DTOs
{

    public class AnyEscolarEditarDto
    {
        public int IdAnyEscolar { get; set; } //identificar únic. Es solicita al crear
        public DateOnly DataIniciCurs { get; set; }
        public DateOnly DataFiCurs { get; set; }
        public Boolean Actiu { get; set; }
        public int DiesPeriode { get; set; } // dies per període
    }

}
