using System.Globalization;
using Frontend.Comandos.Tratamientos;
using Frontend.Models;
using Frontend.Resultados.Tratamientos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Frontend.Controllers;

[ApiController]


public class TratamientoController : ControllerBase
{

    private readonly FarmFoodNutricionContext _context;

    public TratamientoController(FarmFoodNutricionContext context)
    {
        _context = context;
    }


    [HttpPost("api/tratamientos/altaTratAnimal")]
    public async Task<IActionResult> CrearTratamientoPorAnimal([FromBody] ComandoTratamientoAnimal tratAnimal)
    {
        var result = new ResultadoAltaTratamientoPorAnimal();

        var tratamientoAnimal = new TratamientosAnimal
        {
            IdAnimal = tratAnimal.IdAnimal,

            IdTipoTrat = tratAnimal.IdTipoTratamiento,

            Medicacion = tratAnimal.Medicacion,
            FechaInicio = DateOnly.ParseExact(tratAnimal.FechaInicio, "yyyy-MM-dd", null),
            //FechaInicio = tratAnimal.FechaInicio,
            FechaFin = DateOnly.ParseExact(tratAnimal.FechaFin, "yyyy-MM-dd", null)
        };


        await _context.AddAsync(tratamientoAnimal);
        await _context.SaveChangesAsync();

        result.IdTipoTrat = tratamientoAnimal.IdTipoTrat;
        result.Medicacion = tratamientoAnimal.Medicacion;
        result.FechaInicio = tratamientoAnimal.FechaInicio;
        result.FechaFin = tratamientoAnimal.FechaFin;

        result.StatusCode = "200";
        return Ok(result);
        //return Ok("Tratamiento por animal insertado correctamente");
    }

    [HttpPost("api/tratamientos/altaTratLote")]
    public async Task<IActionResult> CrearTratamientoPorLote([FromBody] ComandoTratamientoLote tratLote)
    {

        var tratamientoAnimal = new TratamientosAnimal
        {
            IdAnimal = tratLote.IdLote,

            IdTipoTrat = tratLote.IdTipoTratamiento,

            Medicacion = tratLote.Medicacion,
            FechaInicio = DateOnly.ParseExact(tratLote.FechaInicio, "yyyy-MM-dd", null),
            //FechaInicio = tratAnimal.FechaInicio,
            FechaFin = DateOnly.ParseExact(tratLote.FechaFin, "yyyy-MM-dd", null)
        };


        await _context.AddAsync(tratamientoAnimal);
        await _context.SaveChangesAsync();

        return Ok("Tratamiento por animal insertado correctamente");
    }

    [HttpPost("api/tratamientos/getPorFechas")]
    public async Task<ActionResult<ResultadoListTratamientosPorFecha>> GetTratamientosPorFecha(ComandoFechas comandoFechas)
    {
        // Convertir las fechas del comando a DateOnly
        DateOnly fechaInicio = DateOnly.Parse(comandoFechas.FechaInicio);
        DateOnly fechaFin = DateOnly.Parse(comandoFechas.FechaFin);

        var tratamientosPorFecha = await _context.TratamientosAnimals
            .Where(t => t.FechaInicio >= fechaInicio && t.FechaFin <= fechaFin.AddDays(2))
            .Include(t => t.IdAnimalNavigation)
            .Include(t => t.IdTipoTratNavigation)
            .Select(t => new ResultadoListTratamientosPorFechaItem
            {
                Especie = t.IdAnimalNavigation.IdLoteNavigation.IdRazaNavigation.IdEspecieNavigation.NombreEspecie,
                Raza = t.IdAnimalNavigation.IdLoteNavigation.IdRazaNavigation.NombreRaza,
                NombreTratamiento = t.IdTipoTratNavigation.Decripcion,
                Medicacion = t.Medicacion,
                FechaInicio = t.FechaInicio,
                FechaFin = t.FechaFin,
                DiasDeTratamiento = (t.FechaFin.DayNumber) - (t.FechaInicio.DayNumber)
            })
            .ToListAsync();


        var resultado = new ResultadoListTratamientosPorFecha
        {
            listaTratatamientosPorFecha = tratamientosPorFecha
        };

        if (tratamientosPorFecha == null || tratamientosPorFecha.Count == 0)
        {
            
            resultado.SetError("No se encontraron tratamientos para la fecha especificada");
            
            return Ok(resultado);
            
        }
        
        resultado.StatusCode = "200";
        return Ok(resultado);
    }


}








