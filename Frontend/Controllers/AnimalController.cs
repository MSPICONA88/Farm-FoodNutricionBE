using Frontend.Models;
using Frontend.Resultados.Animales;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Frontend.Controllers;

[ApiController]

public class AnimalController : ControllerBase
{
    private readonly FarmFoodNutricionContext _context;

    public AnimalController(FarmFoodNutricionContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Route("api/animales/traerTodos")]
    public async Task<ActionResult<ResultadoListAnimales>> GetAnimales()
    {
        try
        {
            var result = new ResultadoListAnimales();
            var animales = await _context.Animales.ToListAsync();
            if (animales != null)
            {
                foreach (var ani in animales)
                {
                    var resultAux = new ResultadoListAnimalesItem
                    {
                        IdAnimal = ani.IdAnimal

                    };
                    result.listaAnimales.Add(resultAux);
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
            return BadRequest("Error al obtener los Animales");
        }




    }

    // [HttpGet]
    // [Route("api/animales/traerDisponibles")]
    // public async Task<IActionResult> GetAnimalesDisponibles(int? idEspecie, int? idRaza)
    // {
    //     try
    //     {
    //         var query = _context.Animales
    //             .Include(a => a.IdLoteNavigation)
    //                 .ThenInclude(l => l.IdRazaNavigation)
    //             .Where(a => a.IdLoteNavigation.IdRazaNavigation.IdEspecie == idEspecie);

    //         if (idRaza != null)
    //         {
    //             query = query.Where(a => a.IdLoteNavigation.IdRaza == idRaza);
    //         }

    //         var result = await query
    //             .GroupBy(a => new { a.IdLoteNavigation.IdRazaNavigation.IdEspecieNavigation.NombreEspecie, a.IdLoteNavigation.IdRazaNavigation.NombreRaza })
    //             .Select(g => new AnimalExistence
    //             {
    //                 Especie = g.Key.NombreEspecie,
    //                 Raza = g.Key.NombreRaza,
    //                 CantidadAnimales = g.Sum(a => a.IdLoteNavigation.CantidadAnimales)
    //             })
    //             .ToListAsync();

    //         return Ok(result);
    //     }
    //     catch (Exception e)
    //     {
    //         return BadRequest("Error al obtener los animales disponibles");
    //     }
    // }

    // [HttpGet]
    // [Route("api/animales/traerDisponibles")]
    // public async Task<IActionResult> GetAnimalesDisponibles(int? idEspecie, int? idRaza)
    // {
    //     try
    //     {
    //         var query = _context.Animales
    //             .Include(a => a.IdLoteNavigation)
    //             .ThenInclude(l => l.IdRazaNavigation)
    //             .Where(a => a.IdLoteNavigation.IdRazaNavigation.IdEspecie == idEspecie);

    //         if (idEspecie != null)
    //         {
    //             query = query.Where(a => a.IdLoteNavigation.IdRazaNavigation.IdEspecie == idEspecie);
    //         }

    //         if (idRaza != null)
    //         {
    //             query = query.Where(a => a.IdLoteNavigation.IdRaza == idRaza);

    //         }

    //         var result = await query
    //             .GroupBy(a => new { a.IdLoteNavigation.IdRazaNavigation.IdEspecieNavigation.NombreEspecie, a.IdLoteNavigation.IdRazaNavigation.NombreRaza })
    //             .Select(g => new AnimalExistence
    //             {
    //                 Especie = g.Key.NombreEspecie,
    //                 Raza = g.Key.NombreRaza,
    //                 CantidadAnimales = g.Sum(a => a.IdLoteNavigation.CantidadAnimales)
    //             })
    //             .ToListAsync();

    //         // Verificar si no se seleccionó una especie específica
    //         if (idEspecie == null)
    //         {
    //             // Obtener todas las especies y razas
    //             var especies = await _context.Especies.ToListAsync();
    //             var razas = await _context.Razas.ToListAsync();

    //             // Generar todas las combinaciones de especie y raza
    //             var combinaciones = especies.SelectMany(especie => razas.Select(raza => new { Especie = especie, Raza = raza }));

    //             // Verificar si se seleccionó una raza específica
    //             if (idRaza != null)
    //             {
    //                 // Filtrar las combinaciones por la raza seleccionada
    //                 combinaciones = combinaciones.Where(c => c.Raza.IdRaza == idRaza);
    //             }

    //             // Agregar las combinaciones faltantes a la lista de resultados
    //             foreach (var combinacion in combinaciones)
    //             {
    //                 var existe = result.Any(r => r.Especie == combinacion.Especie.NombreEspecie && r.Raza == combinacion.Raza.NombreRaza);
    //                 if (!existe)
    //                 {
    //                     result.Add(new AnimalExistence
    //                     {
    //                         Especie = combinacion.Especie.NombreEspecie,
    //                         Raza = combinacion.Raza.NombreRaza,
    //                         CantidadAnimales = 0
    //                     });
    //                 }
    //             }
    //         }

    //         return Ok(result);
    //     }
    //     catch (Exception e)
    //     {
    //         return BadRequest("Error al obtener los animales disponibles");
    //     }
    // }

    // [HttpGet]
    // [Route("api/animales/traerDisponibles")]
    // public async Task<IActionResult> GetAnimalesDisponibles(int? idEspecie, int? idRaza)
    // {
    //     try
    //     {
    //         var query = _context.Animales
    //             .Include(a => a.IdLoteNavigation)
    //             .ThenInclude(l => l.IdRazaNavigation)
    //             .Where(a => a.IdLoteNavigation.IdRazaNavigation.IdEspecie == idEspecie);
    //         if (idEspecie != null)
    //         {
    //             query = query.Where(a => a.IdLoteNavigation.IdRazaNavigation.IdEspecie == idEspecie);
    //         }

    //         if (idRaza != null)
    //         {
    //             query = query.Where(a => a.IdLoteNavigation.IdRaza == idRaza);
    //         }

    //         var result = await query
    //             .GroupBy(a => new { a.IdLoteNavigation.IdRazaNavigation.IdEspecieNavigation.NombreEspecie, a.IdLoteNavigation.IdRazaNavigation.NombreRaza })
    //             .Select(g => new AnimalExistence
    //             {
    //                 Especie = g.Key.NombreEspecie,
    //                 Raza = g.Key.NombreRaza,
    //                 CantidadAnimales = g.Sum(a => a.IdLoteNavigation.CantidadAnimales)
    //             })
    //             .ToListAsync();

    //         // Verificar si no se seleccionó una especie específica
    //         if (idEspecie == null)
    //         {
    //             // Obtener todas las especies y razas
    //             var especies = await _context.Especies.ToListAsync();
    //             var razas = await _context.Razas.ToListAsync();

    //             // Generar todas las combinaciones de especie y raza
    //             var combinaciones = especies.SelectMany(especie => razas.Select(raza => new { Especie = especie, Raza = raza }));

    //             // Verificar si se seleccionó una raza específica
    //             if (idRaza != null)
    //             {
    //                 // Filtrar las combinaciones por la raza seleccionada
    //                 combinaciones = combinaciones.Where(c => c.Raza.IdRaza == idRaza);
    //             }

    //             // Agregar las combinaciones faltantes a la lista de resultados
    //             foreach (var combinacion in combinaciones)
    //             {
    //                 var existe = result.Any(r => r.Especie == combinacion.Especie.NombreEspecie && r.Raza == combinacion.Raza.NombreRaza);
    //                 if (!existe)
    //                 {
    //                     var cantidadAnimales = await _context.Animales
    //                         .Where(a => a.IdLoteNavigation.IdRazaNavigation.IdEspecie == combinacion.Especie.IdEspecie && a.IdLoteNavigation.IdRaza == combinacion.Raza.IdRaza)
    //                         .SumAsync(a => a.IdLoteNavigation.CantidadAnimales);

    //                     result.Add(new AnimalExistence
    //                     {
    //                         Especie = combinacion.Especie.NombreEspecie,
    //                         Raza = combinacion.Raza.NombreRaza,
    //                         CantidadAnimales = cantidadAnimales
    //                     });
    //                 }
    //             }
    //         }

    //         return Ok(result);
    //     }
    //     catch (Exception e)
    //     {
    //         return BadRequest("Error al obtener los animales disponibles");
    //     }
    // }


    [HttpGet]
    [Route("api/animales/traerTodosDisponibles")]
    public async Task<IActionResult> GetAnimalesDisponiblesTodos()
    {
        try
        {
            var result = await _context.Lotes
                .Include(l => l.IdRazaNavigation)
                    .ThenInclude(r => r.IdEspecieNavigation)
                .GroupBy(l => new { l.IdRazaNavigation.IdEspecieNavigation.NombreEspecie, l.IdRazaNavigation.NombreRaza })
                .Select(g => new
                {
                    Especie = g.Key.NombreEspecie,
                    Raza = g.Key.NombreRaza,
                    Sum = g.Sum(l => l.CantidadActual)
                })
                .ToListAsync();

            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest("Error al obtener los animales disponibles");
        }
    }

    [HttpGet]
    [Route("api/animales/traerDisponibles")]
    public async Task<IActionResult> GetAnimalesDisponibles(int? idEspecie = null, int? idRaza = null)
    {
        try
        {
            IQueryable<Lote> query = _context.Lotes
                .Include(l => l.IdRazaNavigation)
                    .ThenInclude(r => r.IdEspecieNavigation);

            if (idEspecie != null)
            {
                query = query.Where(l => l.IdRazaNavigation.IdEspecie == idEspecie);
            }

            if (idRaza != null)
            {
                query = query.Where(l => l.IdRazaNavigation.IdRaza == idRaza);
            }

            var result = await query
                .GroupBy(l => new { l.IdRazaNavigation.IdEspecieNavigation.NombreEspecie, l.IdRazaNavigation.NombreRaza })
                .Select(g => new
                {
                    Especie = g.Key.NombreEspecie,
                    Raza = g.Key.NombreRaza,
                    Sum = g.Sum(l => l.CantidadActual)
                })
                .ToListAsync();

            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest("Error al obtener los animales disponibles");
        }
    }


}
