using System.Net;
using Frontend.Comandos.Lotes;
using Frontend.Comandos.Tratamientos;
using Frontend.Comandos.Usuarios;
using Frontend.Models;
using Frontend.Resultados.Lotes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Frontend.Controllers;

[ApiController]

public class LoteController : ControllerBase
{
    private readonly FarmFoodNutricionContext _context;

    public LoteController(FarmFoodNutricionContext context)
    {
        _context = context;
    }


    [HttpPost]
    [Route("api/lote/alta")]
    public async Task<ActionResult<ResultadoAltaLote>> AltaLote([FromBody] ComandoLote comando)
    {
        var finalidad = await _context.Finalidades.FindAsync(comando.IdFinalidad);
        var raza = await _context.Razas.FindAsync(comando.IdRaza);
        var result = new ResultadoAltaLote();
        if (finalidad == null || raza == null)
        {
            result.SetError("Finalidad o Raza no encontrado");
            result.StatusCode = "500";
            return Ok(result);
        }

        var lote = new Lote
        {
            FechaIngreso = DateOnly.FromDateTime(DateTime.Today),
            CantidadAnimales = comando.CantidadAnimales,
            PesoTotal = comando.PesoTotal,
            IdFinalidad = comando.IdFinalidad,
            IdRaza = comando.IdRaza,
            EdadMeses = comando.EdadMeses,
        };

        await _context.AddAsync(lote);
        await _context.SaveChangesAsync();

        result.FechaIngreso = lote.FechaIngreso;
        result.CantidadAnimales = lote.CantidadAnimales;
        result.PesoTotal = lote.PesoTotal;
        result.Finalidad = lote.IdFinalidadNavigation.NombreFinalidad;
        result.Raza = lote.IdRazaNavigation.NombreRaza;
        result.EdadMeses = lote.EdadMeses;

        // Insertar los animales correspondientes al nuevo lote
        for (int i = 1; i <= lote.CantidadAnimales; i++)
        {
            var animal = new Animale
            {
                IdLote = lote.IdLote
            };
            await _context.AddAsync(animal);
        }
        await _context.SaveChangesAsync();

        result.StatusCode = "200";
        return Ok(result);
    }

    [HttpPost("api/lote/getLotesPorFechas")]
    public async Task<ActionResult<ResultadoListLotesPorFecha>> GetTratamientosPorFecha(ComandoFechas comandoFechas)
    {
        // Convertir las fechas del comando a DateOnly
        DateOnly fechaInicio = DateOnly.Parse(comandoFechas.FechaInicio);
        DateOnly fechaFin = DateOnly.Parse(comandoFechas.FechaFin);

        var lotesPorFecha = await _context.Lotes
            .Where(l => l.FechaIngreso >= fechaInicio && l.FechaIngreso <= fechaFin)
            .Include(l => l.IdFinalidadNavigation)
            .Include(l => l.IdRazaNavigation)
            .OrderByDescending(l => l.FechaIngreso)
            .Select(l => new ResultadoListLotesPorFechaItem
            {
                IdLote = l.IdLote,
                FechaIngreso = l.FechaIngreso,
                CantidadAnimales = l.CantidadAnimales,
                PesoTotal = l.PesoTotal,
                Finalidad = l.IdFinalidadNavigation.NombreFinalidad,
                Especie = l.IdRazaNavigation.IdEspecieNavigation.NombreEspecie,
                Raza = l.IdRazaNavigation.NombreRaza,
                EdadMeses = l.EdadMeses
            })
            .ToListAsync();

        var resultado = new ResultadoListLotesPorFecha
        {
            listaLotesPorFecha = lotesPorFecha
        };

        return resultado;
    }

    [HttpPut("api/lote/editarLote/{id}")]
    public async Task<ResultadoUpdateLote> EditarLote(int id, ComandoUpdateLote comandoLote)
    {
        var result = new ResultadoUpdateLote();
        var lote = await _context.Lotes.FindAsync(id);
        if (lote == null)
        {
            result.SetError("No se encontró el id de lote");
            result.StatusCode = "404";
            return result;
        }

        lote.FechaIngreso = DateOnly.Parse(comandoLote.FechaIngreso);
        lote.CantidadAnimales = comandoLote.CantidadAnimales;
        lote.PesoTotal = comandoLote.PesoTotal;
        lote.IdFinalidad = comandoLote.IdFinalidad;
        lote.IdRaza = comandoLote.IdRaza;
        lote.EdadMeses = comandoLote.EdadMeses;


        // lote.FechaIngreso = lote.FechaIngreso;
        // lote.CantidadAnimales = lote.CantidadAnimales;
        // lote.PesoTotal = lote.PesoTotal;
        // lote.IdFinalidad = lote.IdFinalidad;
        // lote.IdRaza = lote.IdRaza;
        // lote.EdadMeses = lote.EdadMeses;


        _context.Lotes.Update(lote);

        var resultadoUpdate = await _context.SaveChangesAsync();


        if (resultadoUpdate < 1)
        {
            result.SetError("No se pudo actualizar el lote");
            result.StatusCode = "404";
            return result;
        }


        result.FechaIngreso = lote.FechaIngreso;
        result.CantidadAnimales = lote.CantidadAnimales;
        result.PesoTotal = lote.PesoTotal;
        result.IdFinalidad = lote.IdFinalidad;
        result.IdRaza = lote.IdRaza;
        result.EdadMeses = lote.EdadMeses;
        result.StatusCode = "200";
        return result;
    }

    [HttpGet]
    [Route("api/lote/lotePorId/{idLote}")]

    public async Task<ActionResult<ResultadoLotePorId>> LotePorId(int idLote)
    {
        var lote = await _context.Lotes.Where(l => l.IdLote == idLote).Include(l => l.IdFinalidadNavigation)
            .Include(l => l.IdRazaNavigation).FirstOrDefaultAsync();
        var result = new ResultadoLotePorId();

        if (lote == null)
        {
            // Si el nombre del alimento ya existe, retornar un mensaje de error
            result.SetError("No se encontró lote");
            //result.StatusCode("500");
            return Ok(result);
            //return BadRequest("El nombre del alimento ya existe");
        }


        result.IdLote = lote.IdLote;
        result.FechaIngreso = lote.FechaIngreso;
        result.CantidadAnimales = lote.CantidadAnimales;
        result.PesoTotal = lote.PesoTotal;
        result.IdFinalidad = lote.IdFinalidad;
        result.IdEspecie = lote.IdRazaNavigation.IdEspecie;
        result.IdRaza = lote.IdRaza;
        result.EdadMeses = lote.EdadMeses;

        return result;
    }

    [HttpGet("api/lote/getLotesPorEspecie")]
    public async Task<ActionResult<ResultadoListLotesPorEspecie>> GetLotesPorEspecie(int idEspecie)
    {
        var lotesPorEspecie = await _context.Lotes
            .Where(l => l.IdRazaNavigation.IdEspecie == idEspecie)
            .Include(l => l.IdFinalidadNavigation)
            .Include(l => l.IdRazaNavigation.IdEspecieNavigation)
            .OrderByDescending(l => l.FechaIngreso)
            .Select(l => new ResultadoListLotesPorEspecieItem
            {
                IdLote = l.IdLote,
                FechaIngreso = l.FechaIngreso,
                CantidadAnimales = l.CantidadAnimales,
                PesoTotal = l.PesoTotal,
                Finalidad = l.IdFinalidadNavigation.NombreFinalidad,
                Especie = l.IdRazaNavigation.IdEspecieNavigation.NombreEspecie,
                Raza = l.IdRazaNavigation.NombreRaza,
                EdadMeses = l.EdadMeses,
                PesoPromedioPorAnimal = l.PesoTotal / l.CantidadAnimales
            })
            .ToListAsync();

        var resultado = new ResultadoListLotesPorEspecie
        {
            ListaLotesPorEspecie = lotesPorEspecie
        };

        return resultado;
    }

    [HttpDelete("api/lote/borrarLote/{id}")]
    public async Task<ActionResult<ResultadoBorrarLote>> BorrarLote(int id)
    {
        var resultado = new ResultadoBorrarLote();

        var lote = await _context.Lotes.Include(l => l.Animales).FirstOrDefaultAsync(l => l.IdLote == id);

        if (lote == null)
        {
            resultado.SetError("No se encontró el lote con el ID especificado");
            resultado.StatusCode = "404";
            return resultado;
        }

        // Eliminar los animales asociados al lote
        _context.Animales.RemoveRange(lote.Animales);

        // Eliminar el lote
        _context.Lotes.Remove(lote);

        var resultadoDelete = await _context.SaveChangesAsync();

        if (resultadoDelete < 1)
        {
            resultado.SetError("No se pudo borrar el lote");
            resultado.StatusCode = "404";
            return resultado;
        }

        resultado.IdLote= lote.IdLote;
        resultado.StatusCode = "200";
        return Ok(resultado);
    }




}

