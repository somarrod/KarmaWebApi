namespace KarmaWebAPI.DTOs
{

    public class PrivilegiEditarDto
    {
        public int IdPrivilegi { get; set; } //identificar únic. Es solicita al crear
        public int Nivell { get; set; }
        public string Descripcio { get; set; }
        
        public string EsIndividualGrup { get; set; }
        
        public int IdAnyEscolar { get; set; }
    }

}
