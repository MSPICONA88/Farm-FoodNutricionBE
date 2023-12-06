using Frontend.Comandos.Tratamientos;
using Frontend.Models;
using Frontend.Resultados.Especies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Frontend.Controllers;

[ApiController]

public class EspecieController : ControllerBase
{
    private readonly FarmFoodNutricionContext _context;

    public EspecieController(FarmFoodNutricionContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Route("api/especie/traerTodas")]

    public async Task<ActionResult<ResultadoListEspecies>> GetEspecies()

    {
        try
        {
            var result = new ResultadoListEspecies();
            var especies = await _context.Especies.ToListAsync();
            if (especies != null)
            {
                foreach (var esp in especies)
                {
                    var resultAux = new ResultadoListEspeciesItem
                    {
                        IdEspecie = esp.IdEspecie,
                        NombreEspecie = esp.NombreEspecie

                    };
                    result.listaEspecies.Add(resultAux);
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
            return BadRequest("Error al obtener las especies");
        }
    }

    [HttpGet("api/reporte/especie/{id}")]
    public async Task<IActionResult> GetEspecieData(int id)
    {
        var especie = await _context.Especies
            .Include(e => e.Razas)
            .ThenInclude(r => r.Lotes)
            .FirstOrDefaultAsync(e => e.IdEspecie == id);

        if (especie == null)
        {
            return NotFound("ID de especie no válido.");
        }

        var cantidadInicial = Math.Round((decimal)especie.Razas.Sum(r => r.Lotes.Sum(l => l.CantidadAnimales)), 2);
        var cantidadActual= Math.Round((decimal)especie.Razas.Sum(r => r.Lotes.Sum(l => l.CantidadActual)), 2);
        var pesoIngreso = Math.Round((decimal)especie.Razas.Sum(r => r.Lotes.Sum(l => l.PesoIngreso)), 2);
        
        var pesoPromedio = especie.Razas.Any()
            ? especie.Razas.Sum(r => r.Lotes.Sum(l => l.PesoIngreso)) / especie.Razas.Sum(r => r.Lotes.Count)
            : 0;
        var pesoActualAprox= Math.Round((decimal)pesoPromedio*cantidadInicial,2);
        var especieData = new
        {
            CantidadInicial = cantidadInicial,
            CantidadActual= cantidadActual,
            PesoIngreso = pesoIngreso,
            PesoPromedio = Math.Round((decimal)pesoPromedio,2),
            PesoActualAprox=pesoActualAprox
        };

        return Ok(especieData);
    }

    [HttpGet("api/reporte/especie")]
    public async Task<IActionResult> GetAllEspeciesData()
    {
        var cantidadActual = await _context.Lotes.SumAsync(l => l.CantidadActual);
        var cantidadInicial = await _context.Lotes.SumAsync(l => l.CantidadAnimales);
        var pesoIngreso = await _context.Lotes.SumAsync(l => l.PesoIngreso);
        var pesoPromedio = await _context.Lotes.SumAsync(l => l.PesoIngreso)/cantidadInicial;
        var pesoActualAprox= pesoPromedio*cantidadActual;

        var especiesData = new
        {
            CantidadActual = cantidadActual,
            CantidadInicial= cantidadInicial,
            PesoIngreso = pesoIngreso,
            PesoPromedio = pesoPromedio,
            PesoActualAprox= pesoActualAprox
        };

        return Ok(especiesData);
    }

    // [HttpGet("api/reporte/animalesPorEspecie")]
    // public ActionResult<List<AnimalesPorEspecieDTO>> GetAnimalesPorEspecie()
    // {
    //     var animalesPorEspecie = _context.Lotes
    //         .Select(l => new
    //         {
    //             Especie = l.IdRazaNavigation.IdEspecieNavigation.NombreEspecie,
    //             Mes = l.FechaIngreso.Month,
    //             Año = l.FechaIngreso.Year,
    //             CantidadAnimales = l.CantidadAnimales
    //         })
    //         .GroupBy(a => new { a.Especie, a.Mes, a.Año })
    //         .Select(g => new AnimalesPorEspecieDTO
    //         {
    //             Especie = g.Key.Especie,
    //             Mes = g.Key.Mes,
    //             Año = g.Key.Año,
    //             CantidadAnimales = g.Sum(a => a.CantidadAnimales)
    //         })
    //         .ToList();

    //     return animalesPorEspecie;
    // }


    [HttpGet("api/reporte/animalesPorEspecie")]
    public ActionResult<List<AnimalesPorEspecieDTO>> GetAnimalesPorEspecie()
    {
        var animalesPorEspecie = _context.Lotes
            .Select(l => new
            {
                Especie = l.IdRazaNavigation.IdEspecieNavigation.NombreEspecie,
                Mes = l.FechaIngreso.Month,
                Año = l.FechaIngreso.Year,
                CantidadInicial= l.CantidadAnimales,
                CantidadActual = l.CantidadActual
            })
            .GroupBy(a => new { a.Especie, a.Mes, a.Año })
            .Select(g => new AnimalesPorEspecieDTO
            {
                Especie = g.Key.Especie,
                Mes = g.Key.Mes,
                Año = g.Key.Año,
                CantidadInicial = g.Sum(a => a.CantidadInicial),
                CantidadActual = g.Sum(a => a.CantidadActual)
            })
            .OrderBy(a => a.Año) // Ordenar por año ascendente
            .ThenBy(a => a.Mes) // Luego ordenar por mes ascendente
            .ToList();

        return animalesPorEspecie;
    }


    // [HttpPost("api/reporte/animalesPorEspecieFecha")]
    // public ActionResult<List<AnimalesPorEspecieDTO>> GetAnimalesPorEspecie(ComandoFechas comandoFechas)
    // {
    //     if (string.IsNullOrEmpty(comandoFechas.FechaInicio) || string.IsNullOrEmpty(comandoFechas.FechaFin))
    //     {
    //         // Manejar el caso de fechas vacías o nulas
    //         // Por ejemplo, puedes devolver un error o un resultado vacío
    //         return BadRequest("Fechas inválidas");
    //     }
    //     DateOnly fechaInicio = DateOnly.Parse(comandoFechas.FechaInicio);
    //     DateOnly fechaFin = DateOnly.Parse(comandoFechas.FechaFin);

    //     var animalesPorEspecie = _context.Lotes
    //         .Where(l => l.FechaIngreso >= fechaInicio && l.FechaIngreso <= fechaFin)
    //         .Select(l => new
    //         {
    //             Especie = l.IdRazaNavigation.IdEspecieNavigation.NombreEspecie,
    //             Mes = l.FechaIngreso.Month,
    //             Año = l.FechaIngreso.Year,
    //             CantidadAnimales = l.CantidadAnimales
    //         })
    //         .GroupBy(a => new { a.Especie, a.Mes, a.Año })
    //         .Select(g => new AnimalesPorEspecieDTO
    //         {
    //             Especie = g.Key.Especie,
    //             Mes = g.Key.Mes,
    //             Año = g.Key.Año,
    //             CantidadAnimales = g.Sum(a => a.CantidadAnimales)
    //         })
    //         .ToList();

    //     return animalesPorEspecie;
    // }

    [HttpPost("api/reporte/animalesPorEspecieFecha")]
    public ActionResult<List<AnimalesPorEspecieDTO>> GetAnimalesPorEspecie(ComandoFechas comandoFechas)
    {
        if (string.IsNullOrEmpty(comandoFechas.FechaInicio) || string.IsNullOrEmpty(comandoFechas.FechaFin))
        {
            // Manejar el caso de fechas vacías o nulas
            // Por ejemplo, puedes devolver un error o un resultado vacío
            return BadRequest("Fechas inválidas");
        }
        DateOnly fechaInicio = DateOnly.Parse(comandoFechas.FechaInicio);
        DateOnly fechaFin = DateOnly.Parse(comandoFechas.FechaFin);

        var animalesPorEspecie = _context.Lotes
            .Where(l => l.FechaIngreso >= fechaInicio && l.FechaIngreso <= fechaFin)
            .Select(l => new
            {
                Especie = l.IdRazaNavigation.IdEspecieNavigation.NombreEspecie,
                Mes = l.FechaIngreso.Month,
                Año = l.FechaIngreso.Year,
                CantidadActual = l.CantidadActual
            })
            .GroupBy(a => new { a.Especie, a.Mes, a.Año })
            .Select(g => new AnimalesPorEspecieDTO
            {
                Especie = g.Key.Especie,
                Mes = g.Key.Mes,
                Año = g.Key.Año,
                CantidadActual = g.Sum(a => a.CantidadActual)
            })
            .OrderBy(a => a.Año) // Ordenar por año ascendente
            .ThenBy(a => a.Mes) // Luego ordenar por mes ascendente
            .ToList();

        return animalesPorEspecie;
    }


}

