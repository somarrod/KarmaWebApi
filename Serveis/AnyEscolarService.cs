using KarmaWebAPI.Data;
using KarmaWebAPI.DTOs;
using KarmaWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KarmaWebAPI.Serveis
{

    public class AnyEscolarService : IAnyEscolarService
    {
        private readonly DatabaseContext _context;
        private readonly IPrivilegiService _privilegiService;

        public AnyEscolarService(DatabaseContext context, IPrivilegiService privilegiService)
        {
            _context = context;
            _privilegiService = privilegiService;
        }

        public async Task<ActionResult<AnyEscolar>> CrearAnyEscolarAsync(AnyEscolarCrearDto anyEscolarDto)
        {
            if (anyEscolarDto.DiesPeriode < 7) {
                return new ConflictObjectResult("Els dies de periode han de ser valors majors o iguals a 7."); // Use ConflictObjectResult
            }

            var anyEscolar = new AnyEscolar
            {
                IdAnyEscolar = int.Parse(anyEscolarDto.DataIniciCurs.Year.ToString() + anyEscolarDto.DataFiCurs.Year.ToString()),
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

            for (var i = 0; i <= 3; i++)
            {
               

                var privilegiDto = new PrivilegiCrearDto
                {
                    Descripcio = "Privilegi TEST" + i,
                    Nivell = i,
                    EsIndividualGrup = "I",
                    IdAnyEscolar = anyEscolar.IdAnyEscolar
                };
                var resPrivilegi = await _privilegiService.CrearPrivilegiAsync(privilegiDto);

                if (resPrivilegi.Result is not OkObjectResult)
                {
                    return new StatusCodeResult(500); 
                }
            }

            return new OkObjectResult(anyEscolar);
        }

    }

}
