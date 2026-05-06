
public interface IKarmaAlumneService
{
    // -------------------------------------------------
    // A) Operacions PER AVALUACIÓ
    // -------------------------------------------------

    // Crear KarmaAlumne per a tots els alumnes en una avaluació
    Task CrearPerAvaluacioAsync(
        long idAvaluacio,
        double puntsInicials);

    // Crear KarmaAlumne copiant des de una avaluació anterior
    Task CopiarPerAvaluacioAsync(
        long idAvaluacioActual,
        long idAvaluacioAnterior);

    // Calcular nota final per a una avaluació
    Task CalcularNotaFinalAsync(long idAvaluacio);


    // -------------------------------------------------
    // B) Operacions PER ALUMNE
    // -------------------------------------------------

    Task CrearPerAlumneIAvaluacioAsync(
        string nia,
        long idAvaluacio,
        double puntsInicials);


    // Alumne nou → crear KarmaAlumne només per a UNA avaluació
    Task CrearPerAlumneDesdeAvaluacioEnCursAsync(
        string nia,
        int idAnyEscolar);

}
