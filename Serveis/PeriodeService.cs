
using KarmaWebAPI.Data;
using KarmaWebAPI.DTOs;
using KarmaWebAPI.Models;
using KarmaWebAPI.Serveis.Interfaces;
using Microsoft.EntityFrameworkCore;

public class PeriodeService: IPeriodeService
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


    public async Task<Periode> TCrearAsync(PeriodeTCREARDTO periodeDto)
    {
        var anyEscolar = await _context.AnyEscolar.FindAsync(periodeDto.IdAnyEscolar);
        if (anyEscolar == null)
        {
            throw new InvalidOperationException($"Any escolar amb Id {periodeDto.IdAnyEscolar} no trobat");
        }

        DateOnly dataFi;
        if (anyEscolar.DataFiCurs > periodeDto.DataInici.AddDays(anyEscolar.DiesPeriode))
        {
            dataFi = periodeDto.DataInici.AddDays(anyEscolar.DiesPeriode);
        }
        else
        {
            dataFi = anyEscolar.DataFiCurs;
        }

        var periode = new Periode
        {
            IdAnyEscolar = periodeDto.IdAnyEscolar,
            DataInici = periodeDto.DataInici,
            DataFi = dataFi
        };

        _context.Periode.Add(periode);
        await _context.SaveChangesAsync();

        if (periode.DataFi < anyEscolar.DataFiCurs)
        {
            var nextPeriodeDto = new PeriodeTCREARDTO
            {
                IdAnyEscolar = periode.IdAnyEscolar,
                DataInici = periode.DataFi.AddDays(1)
            };
            await TCrearAsync(nextPeriodeDto);
        }

        return periode;
    }
}




