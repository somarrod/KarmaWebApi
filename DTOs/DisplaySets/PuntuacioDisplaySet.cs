namespace KarmaWebAPI.DTOs.DisplaySets
{
    public class PuntuacioDisplaySet
    {
        public long IdPuntuacio { get; set; }

        // Alumne
        public string NIA { get; set; } = string.Empty;
        public string NomAlumne { get; set; } = string.Empty;

        // Professor
        public string IdProfessor { get; set; } = string.Empty;
        public string NomProfessor { get; set; } = string.Empty;

        // Categoria
        public long IdCategoria { get; set; }
        public string DescripcioCategoria { get; set; } = string.Empty;

        // Classe
        public long IdClasse { get; set; }
        public string NomClasse { get; set; } = string.Empty;

        // Grup
        public long? IdGrup { get; set; }
        public string? NomGrup { get; set; }

        // Dades puntuació
        public double NumPunts { get; set; }
        public string Tipus { get; set; } = string.Empty;
        public string Motiu { get; set; } = string.Empty;
        public string? DescripcioAdicional { get; set; }

        // Dates
        public DateOnly DataEvent { get; set; }
        public DateTime DataCreacio { get; set; }

        // Avaluació
        public long IdAvaluacio { get; set; }
        public string NomAvaluacio { get; set; } = string.Empty;
        public int IdAnyEscolar { get; set; }

        // Karma alumne
        public double KarmaActualPunts { get; set; }
        public string KarmaActualColor { get; set; } = string.Empty;
    }
   
}
