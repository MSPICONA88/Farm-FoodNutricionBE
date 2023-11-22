using Frontend.Models;
using Frontend.Resultados.Tratamientos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Frontend.Controllers;

[ApiController]

public class TiposTratamientoController : ControllerBase
{

private readonly FarmFoodNutricionContext _context;

    public TiposTratamientoController(FarmFoodNutricionContext context)
    {
        _context=context;
    }

[HttpGet]
[Route("api/tratamiento/traerTipo")]
    public async Task<ActionResult<ResultadoListTiposTratamiento>> GetTiposTratamiento()
    {
        try
        {
            var result= new ResultadoListTiposTratamiento();
            var tiposTrat=  await _context.TiposTratamientos.ToListAsync();
            if(tiposTrat!=null){
                foreach (var tipo in tiposTrat){
                    var resultAux = new ResultadoListTiposTratamientoItem
                    {
                        IdTipoTratamiento= tipo.IdTipoTrat,
                        Descripcion = tipo.Decripcion
                        
                    };
                    result.listaTiposTrat.Add(resultAux);
                    result.StatusCode= "200";
                }
                return Ok(result);
            }

            else
            {
                return Ok(result);
            }
        }

        catch(Exception e)
        {
            return BadRequest("Error al obtener los tipos de Tratamiento");
        }

        
        

    }


}
