using Frontend.Models;
using Frontend.Resultados.Usuarios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Frontend.Resultados.Usuarios.ResultadoListRoles;

namespace Frontend.Controllers;

[ApiController]

public class RolController : ControllerBase
{
    private readonly FarmFoodNutricionContext _context;

    public RolController(FarmFoodNutricionContext context)
    {
        _context=context;
    }

[HttpGet]
[Route("api/rol/traerRol")]
    public async Task<ActionResult<ResultadoListRoles>> GetRoles()
    {
        try
        {
            var result= new ResultadoListRoles();
            var roles=  await _context.Roles.ToListAsync();
            if(roles!=null){
                foreach (var rol in roles){
                    var resultAux = new ResultadoListRolesItem
                    {
                        IdRol= rol.IdRol,
                        NombreRol = rol.NombreRol
                        
                    };
                    result.listaRoles.Add(resultAux);
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
            return BadRequest("Error al obtener los roles");
        }

        
        

    }
}
