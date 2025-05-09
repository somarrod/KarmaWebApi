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
            //GETONE (AnyEscolar.ConfiguraKarma.ColorNivell)
            //WHERE (AnyEscolar.ConfiguraKarma.KarmaMinim <= MIN(AlumnesEnGrup.PuntuacioTotal) AND
            //      AnyEscolar.ConfiguraKarma.KarmaMaxim >= MIN(AlumnesEnGrup.PuntuacioTotal))

           // Obtenir la puntuació mínima dels alumnes en el grup específic
          var puntuacioMinimaGrup = await _context.AlumneEnGrup
                                  .Where(a => a.IdGrup == idGrup && a.IdAnyEscolar == idAnyEscolar)
                                  .MinAsync(a => a.PuntuacioTotal);

            // Obtenir l'any escolar del grup específic
            var grup = await _context.Grup
                                     .Where(g => g.IdGrup == idGrup && g.IdAnyEscolar == idAnyEscolar)
                                     .FirstOrDefaultAsync();


            // Obtenir el color del nivell de karma basat en la puntuació mínima i l'any escolar
            var karmaConfig = await _context.ConfiguracioKarma
                                    .Where(k => k.IdAnyEscolar == grup.IdAnyEscolar)
                                    .FirstOrDefaultAsync(k => k.KarmaMinim <= puntuacioMinimaGrup && k.KarmaMaxim >= puntuacioMinimaGrup);
            if (karmaConfig != null)
            {
                return karmaConfig.ColorNivell;
            }

            return "No trobat!"; // O un valor per defecte si no es troba cap coincidència
        }

    }

}
