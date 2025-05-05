using KarmaWebAPI.Data;
using KarmaWebAPI.DTOs;
using KarmaWebAPI.Models;
using KarmaWebAPI.Serveis.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KarmaWebAPI.Serveis
{

    public class AnyEscolarService : IAnyEscolarService
    {
        private readonly DatabaseContext _context;
        private readonly IPeriodeService _periodeService;

        public AnyEscolarService(DatabaseContext context, IPeriodeService periodeService)
        {
            _context = context;
            _periodeService = periodeService;
        }

        public async Task<ActionResult<AnyEscolar>> CrearAnyEscolarAsync(AnyEscolarCrearDto anyEscolarDto)
        {
            if (anyEscolarDto.DiesPeriode < 7) {
                return new ConflictObjectResult("Els dies de periode han de ser valors majors o iguals a 7."); // Use ConflictObjectResult
            }

            var anyEscolar = new AnyEscolar
            {
                IdAnyEscolar = int.Parse((anyEscolarDto.DataIniciCurs.Year -2000).ToString() + (anyEscolarDto.DataFiCurs.Year -2000).ToString()),
                DataIniciCurs = anyEscolarDto.DataIniciCurs,
                DataFiCurs = anyEscolarDto.DataFiCurs,
                Actiu = true,
                DiesPeriode = anyEscolarDto.DiesPeriode,
                Privilegis = new List<Privilegi>()
            };

            _context.AnyEscolar.Add(anyEscolar);
            await _context.SaveChangesAsync();

            return new OkObjectResult(anyEscolar);
        }

        public async Task<ActionResult<AnyEscolar>> TCREARAsync(AnyEscolarCrearDto anyEscolarDto)
        {
            var result = await CrearAnyEscolarAsync(anyEscolarDto);

            if (result.Result is not OkObjectResult okResult)
            {
                return result;
            }

            var anyEscolar = okResult.Value as AnyEscolar;

            await _periodeService.TCrearAsync(new PeriodeTCREARDTO
            {
                IdAnyEscolar = anyEscolar.IdAnyEscolar,
                DataInici = anyEscolar.DataIniciCurs,
            });

            return new OkObjectResult(anyEscolar);
        }

    }

}
