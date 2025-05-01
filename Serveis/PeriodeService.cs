
using KarmaWebAPI.Data;
using KarmaWebAPI.Models;
using Microsoft.EntityFrameworkCore;

public class PeriodeService
{
    private readonly DatabaseContext _context;

    public PeriodeService(DatabaseContext context)
    {
        _context = context;
    }

    public List<Periode> GetPeriodes()
    {
        return _context.Periode.ToList();
    }

    public ICollection<VPrivilegiPeriode> GetPrivilegisPeriode(int idPeriode)
    {
        return _context.VPrivilegiPeriode
                        .Where(v => idPeriode == v.IdPeriode)
                        .ToList();
    }
}
