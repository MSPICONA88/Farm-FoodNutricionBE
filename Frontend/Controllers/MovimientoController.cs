using Frontend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]

public class MovimientoController : ControllerBase
{
    private readonly FarmFoodNutricionContext _context;

    public MovimientoController(FarmFoodNutricionContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Route("api/movimiento/traerTodos")]

    public async Task<ActionResult<ResultadoListMovimientos>> GetMovimientos()

    {
        try
        {
            var result = new ResultadoListMovimientos();
            var movimientos = await _context.TiposMovimientos.ToListAsync();
            if (movimientos != null)
            {
                foreach (var mov in movimientos)
                {
                    var resultAux = new ResultadoListMovimientosItem
                    {
                        IdMovimiento = mov.IdMovimiento,
                        NombreMovimiento = mov.NombreMovimiento,
                        Descripcion= mov.Descripcion

                    };
                    result.listaMovimientos.Add(resultAux);
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
            return BadRequest("Error al obtener los movimientos");
        }
    }
}
