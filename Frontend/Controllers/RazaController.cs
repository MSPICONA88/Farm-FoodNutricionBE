using Frontend.Comandos.Usuarios;
using Frontend.Models;
using Frontend.Resultados.Razas;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Frontend.Controllers;

[ApiController]

public class RazaController : ControllerBase
{
    private readonly FarmFoodNutricionContext _context;

    public RazaController(FarmFoodNutricionContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Route("api/raza/razaPorEspecie/{idEspecie}")]

    public async Task<ActionResult<ResultadoRazaPorEspecie>> RazaPorEspecie(int idEspecie)
    {
        var razas = await _context.Razas.Where(r => r.IdEspecieNavigation.IdEspecie == idEspecie).ToListAsync();
        var result = new ResultadoRazaPorEspecie();

        if (razas == null || razas.Count == 0)
        {
            // Si el nombre del alimento ya existe, retornar un mensaje de error
            result.SetError("No se encontraron razas para la especie especificada");
            //result.StatusCode("500");
            return Ok(result);
            //return BadRequest("El nombre del alimento ya existe");
        }
        foreach (var raza in razas)
        {
            var resultAux = new ResultadoListRazasPorEspecieItem
            {
                IdRaza = raza.IdRaza,
                NombreRaza = raza.NombreRaza

            };
            result.listaRazasPorEspecie.Add(resultAux);
            result.StatusCode = "200";
        }
        
        return Ok(result);
    }

}
