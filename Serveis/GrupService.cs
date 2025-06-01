using System;
using KarmaWebAPI.Data;
using KarmaWebAPI.DTOs;
using KarmaWebAPI.Models;
using KarmaWebAPI.Serveis.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace KarmaWebAPI.Serveis
{

    public class GrupService : IGrupService
    {
        private readonly DatabaseContext _context;

        public GrupService(DatabaseContext context)
        {
            _context = context;
        }


        public async Task<String> calculaKarmaBaseAsync(int idAnyEscolar, String idGrup)
        {
            var grup = await _context.Grup
                         .Where(g => g.IdGrup == idGrup && g.IdAnyEscolar == idAnyEscolar)
                         .FirstOrDefaultAsync();

            if (grup == null) {
                return "El grup introduit no existeix";
            }

            string Karma = "CAP KARMA";
            // Comprovació prèvia: existeix algun alumne en grup?
            var existeixAlgun = await _context.AlumneEnGrup.AnyAsync();
            if (existeixAlgun)
            {
                // Obtenir la puntuació mínima dels alumnes en el grup específic
                   var puntuacioMinimaGrup = await _context.AlumneEnGrup
                                              .Where(a => a.IdGrup == idGrup && a.IdAnyEscolar == idAnyEscolar)
                                              .MinAsync(a => a.PuntuacioTotal);


                    // Obtenir el color del nivell de karma basat en la puntuació mínima i l'any escolar
                    var karmaConfig = await _context.ConfiguracioKarma
                                            .Where(k => k.IdAnyEscolar == idAnyEscolar)
                                            .FirstOrDefaultAsync(k => k.KarmaMinim <= puntuacioMinimaGrup && k.KarmaMaxim >= puntuacioMinimaGrup);


                    if (karmaConfig != null && grup != null)
                    {
                        Karma = karmaConfig.ColorNivell;
                    }
            }
            grup.KarmaBase = Karma;
            _context.Grup.Update(grup);
            await _context.SaveChangesAsync();

            return Karma;
        }

    }

}
