namespace KarmaWebAPI.DTOs
{

    public class PeriodeTCREARDTO
    {
        //No crearem create_instance, sino directament TCREAR
        public DateOnly DataInici { get; set; }
        
        public int IdAnyEscolar { get; set; }
    }

}
