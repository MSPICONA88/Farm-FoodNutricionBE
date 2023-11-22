using Frontend.Models;
using Frontend.Resultados.Finalidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Frontend.Controllers;

[ApiController]

public class FinalidadController : ControllerBase
{
    private readonly FarmFoodNutricionContext _context;

    public FinalidadController(FarmFoodNutricionContext context)
    {
        _context = context;
    }

    
    [HttpGet]
    [Route("api/finalidad/traerTodas")]

    public async Task<ActionResult<ResultadoListFinalidades>> GetFinalidades()

    {
        try
        {
            var result = new ResultadoListFinalidades();
            var finalidad = await _context.Finalidades.ToListAsync();
            if (finalidad != null)
            {
                foreach (var fin in finalidad)
                {
                    var resultAux = new ResultadoListFinalidadesItem
                    {
                        IdFinalidad= fin.IdFinalidad,
                        NombreFinalidad = fin.NombreFinalidad

                    };
                    result.listaFinalidades.Add(resultAux);
                    result.StatusCode = "200";
                }
                return Ok(result);
            }

            else
            {
                return Ok(result);
            }
        }

        catch (Exception e)
        {
            return BadRequest("Error al obtener las finalidades");
        }
    }
}
