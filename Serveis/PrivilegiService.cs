using KarmaWebAPI.Data;
using KarmaWebAPI.DTOs;
using KarmaWebAPI.Models;
using KarmaWebAPI.Serveis.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace KarmaWebAPI.Serveis
{

    public class PrivilegiService : IPrivilegiService
    {
        private readonly DatabaseContext _context;

        public PrivilegiService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<Privilegi>> CrearPrivilegiAsync(PrivilegiCrearDto privilegiDto) // Change return type to IActionResult
        {
            var anyEscolar = await _context.AnyEscolar.FindAsync(privilegiDto.IdAnyEscolar);
            if (anyEscolar == null)
            {
                return new NotFoundObjectResult("Any escolar " + privilegiDto.IdAnyEscolar + " no trobat"); // Use NotFoundObjectResult
            }

            // Verificar si ya existe un Privilegi con el mismo nombre
            var existePrivilegi = await _context.Privilegi
                    .AnyAsync(p => p.Descripcio == privilegiDto.Descripcio);

            if (existePrivilegi)
            {
                return new ConflictObjectResult("Ja existeix un privilegi amb la mateixa descripció."); // Use ConflictObjectResult
            }

            var privilegi = new Privilegi
            {
                Nivell = privilegiDto.Nivell,
                Descripcio = privilegiDto.Descripcio,
                EsIndividualGrup = privilegiDto.EsIndividualGrup,
                IdAnyEscolar = privilegiDto.IdAnyEscolar,
                AnyEscolar = anyEscolar // Asignar la instancia recuperada
            };

            await _context.Privilegi.AddAsync(privilegi);
            await _context.SaveChangesAsync();
            return new OkObjectResult(privilegi.IdPrivilegi); // Return the created object
        }

        public ICollection<VPrivilegiPeriode> GetPrivilegisPeriode(int idPrivilegi)
        {
            return _context.VPrivilegiPeriode
                            .Where(v => idPrivilegi == v.IdPrivilegi)
                            .ToList();
        }
    }

}
