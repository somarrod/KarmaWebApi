using System.Collections.Generic;
using System.Linq;
using KarmaWebAPI.Models;
using KarmaWebAPI.DTOs;
using KarmaWebAPI.Data;

public class ConfiguracioKarmaService
{
    private readonly DatabaseContext _context;

    public ConfiguracioKarmaService(DatabaseContext context)
    {
        _context = context;
    }

    public bool ValidateKarmaRange(int karmaMinim, int karmaMaxim)
    {
        var existingConfigurations = _context.ConfiguracioKarma.ToList();

        foreach (var config in existingConfigurations)
        {
            if ((karmaMinim >= config.KarmaMinim && karmaMinim <= config.KarmaMaxim) ||
                (karmaMaxim >= config.KarmaMinim && karmaMaxim <= config.KarmaMaxim) ||
                (karmaMinim <= config.KarmaMinim && karmaMaxim >= config.KarmaMaxim))
            {
                return false; // Solapament trobat
            }
        }

        return true; // No hi ha solapaments
    }

    public void CrearConfiguracioKarma(ConfiguracioKarmaCrearDTO dto)
    {
        if (!ValidateKarmaRange(dto.KarmaMinim, dto.KarmaMaxim))
        {
            throw new InvalidOperationException("El rang de karma solapa amb una configuració existent.");
        }

        var newConfig = new ConfiguracioKarma
        {
            IdAnyEscolar = dto.IdAnyEscolar,
            KarmaMinim = dto.KarmaMinim,
            KarmaMaxim = dto.KarmaMaxim,
            ColorNivell = dto.ColorNivell,
            NivellPrivilegis = dto.NivellPrivilegis
        };

        _context.ConfiguracioKarma.Add(newConfig);
        _context.SaveChanges();
    }
}
