using System.Collections.Generic;
using System.Linq;
using KarmaWebAPI.Models;
using KarmaWebAPI.DTOs;
using KarmaWebAPI.Data;
using KarmaWebAPI.Serveis.Interfaces;

public class ConfiguracioKarmaService: IConfiguracioKarmaService
{
    private readonly DatabaseContext _context;

    public ConfiguracioKarmaService(DatabaseContext context)
    {
        _context = context;
    }

    public bool ValidateKarmaRange(int idConfiguracioKarma, int karmaMinim, int karmaMaxim)
    {
        var existingConfigurations = _context.ConfiguracioKarma.ToList();

        foreach (var config in existingConfigurations)
        {
            if (config.IdConfiguracioKarma != idConfiguracioKarma && 
               ((karmaMinim >= config.KarmaMinim && karmaMinim <= config.KarmaMaxim) ||
               (karmaMaxim >= config.KarmaMinim && karmaMaxim <= config.KarmaMaxim) ||
               (karmaMinim <= config.KarmaMinim && karmaMaxim >= config.KarmaMaxim)))
            {
                return false; // Solapament trobat
            }
        }

        return true; // No hi ha solapaments
    }


    public async Task CrearConfiguracioKarmaAsync(ConfiguracioKarmaCrearDTO dto)
    {
        if (!ValidateKarmaRange(0, dto.KarmaMinim, dto.KarmaMaxim))
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
        await _context.SaveChangesAsync();

        _context.AlumneEnGrup
            .Where(a => a.IdAnyEscolar == dto.IdAnyEscolar &&
                        a.PuntuacioTotal >= dto.KarmaMinim &&
                        a.PuntuacioTotal <= dto.KarmaMaxim)
            .ToList()
            .ForEach(a => a.Karma = dto.ColorNivell);
        await _context.SaveChangesAsync();
    }


    public async Task EditarConfiguracioKarmaAsync(ConfiguracioKarmaEditarDTO dto)
    {
        if (!ValidateKarmaRange(dto.IdConfiguracioKarma, dto.KarmaMinim, dto.KarmaMaxim))
        {
            throw new InvalidOperationException("El rang de karma solapa amb una configuració existent.");
        }

        var existingConfig = await _context.ConfiguracioKarma.FindAsync(dto.IdConfiguracioKarma);
        if (existingConfig == null)
        {
            throw new InvalidOperationException("La configuració de karma no existeix.");
        }

        existingConfig.IdAnyEscolar = dto.IdAnyEscolar;
        existingConfig.KarmaMinim = dto.KarmaMinim;
        existingConfig.KarmaMaxim = dto.KarmaMaxim;
        existingConfig.ColorNivell = dto.ColorNivell;
        existingConfig.NivellPrivilegis = dto.NivellPrivilegis;

        _context.ConfiguracioKarma.Update(existingConfig);
        await _context.SaveChangesAsync();
    }


}
