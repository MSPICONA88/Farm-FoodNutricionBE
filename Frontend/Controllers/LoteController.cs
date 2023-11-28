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
            PesoIngreso = comando.PesoIngreso,
            IdFinalidad = comando.IdFinalidad,
            IdRaza = comando.IdRaza,
            EdadMeses = comando.EdadMeses,
            FechaEgreso = null,
            PesoEgreso = null,
            CantidadActual = comando.CantidadAnimales

        };

        await _context.AddAsync(lote);
        await _context.SaveChangesAsync();

        result.FechaIngreso = lote.FechaIngreso;
        result.CantidadAnimales = lote.CantidadAnimales;
        result.PesoIngreso = lote.PesoIngreso;
        result.Finalidad = lote.IdFinalidadNavigation.NombreFinalidad;
        result.Raza = lote.IdRazaNavigation.NombreRaza;
        result.EdadMeses = lote.EdadMeses;
        result.CantidadActual = lote.CantidadActual;

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

    //trae todos los lotes incluidos los egresados
    [HttpPost("api/lote/getLotesPorFechas")]
    public async Task<ActionResult<ResultadoListLotesPorFecha>> GetLotesPorFecha(ComandoFechas comandoFechas)
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
                CantidadActual = l.CantidadActual,
                CantidadBajas = l.CantidadAnimales - l.CantidadActual,
                FechaEgreso = l.FechaEgreso,
                PesoIngreso = l.PesoIngreso,
                PesoEgreso = l.PesoEgreso,
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

    [HttpGet("api/lote/getLotesPorFechasDef")]
    public async Task<ActionResult<ResultadoListLotesPorFecha>> GetLotesPorFechaDef()
    {
        string fechaInicioDef = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
        string FechaFinDef = DateTime.Now.ToString("yyyy-MM-dd");

        // Convertir las fechas del comando a DateOnly
        DateOnly fechaInicio = DateOnly.Parse(fechaInicioDef);
        DateOnly fechaFin = DateOnly.Parse(FechaFinDef);

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
                CantidadActual = l.CantidadActual,
                PesoIngreso = l.PesoIngreso,
                Finalidad = l.IdFinalidadNavigation.NombreFinalidad,
                Especie = l.IdRazaNavigation.IdEspecieNavigation.NombreEspecie,
                Raza = l.IdRazaNavigation.NombreRaza,
                EdadMeses = l.EdadMeses,
                CantidadBajas = l.CantidadAnimales - l.CantidadActual
            })
            .ToListAsync();

        var resultado = new ResultadoListLotesPorFecha
        {
            listaLotesPorFecha = lotesPorFecha
        };

        return resultado;
    }

    [HttpPost("api/lote/getLotesDisponibles")]
    public async Task<ActionResult<ResultadoListLotesDispPorFecha>> GetLotesDisponiblesPorFecha(ComandoFechas comandoFechas)
    {

        // Convertir las fechas del comando a DateOnly
        DateOnly fechaInicio = DateOnly.Parse(comandoFechas.FechaInicio);
        DateOnly fechaFin = DateOnly.Parse(comandoFechas.FechaFin);

        var lotesPorFecha = await _context.Lotes
            .Where(l => (l.FechaIngreso >= fechaInicio && l.FechaIngreso <= fechaFin) && (l.FechaEgreso == null))
            .Include(l => l.IdFinalidadNavigation)
            .Include(l => l.IdRazaNavigation)
            .OrderByDescending(l => l.FechaIngreso)
            .Select(l => new ResultadoListLotesDispPorFechaItem
            {
                IdLote = l.IdLote,
                FechaIngreso = l.FechaIngreso,
                CantidadActual = l.CantidadActual,
                PesoIngreso = l.PesoIngreso,
                Finalidad = l.IdFinalidadNavigation.NombreFinalidad,
                Especie = l.IdRazaNavigation.IdEspecieNavigation.NombreEspecie,
                Raza = l.IdRazaNavigation.NombreRaza,
                EdadMeses = l.EdadMeses,
                CantidadBajas = l.CantidadAnimales - l.CantidadActual
            })
            .ToListAsync();

        var resultado = new ResultadoListLotesDispPorFecha
        {
            listaLotesDispPorFecha = lotesPorFecha
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

        // Validar si se ingresó una fecha de egreso



        lote.FechaIngreso = DateOnly.Parse(comandoLote.FechaIngreso);
        lote.CantidadAnimales = comandoLote.CantidadAnimales;
        lote.PesoIngreso = comandoLote.PesoIngreso;
        lote.IdFinalidad = comandoLote.IdFinalidad;
        lote.IdRaza = comandoLote.IdRaza;
        lote.EdadMeses = comandoLote.EdadMeses;
        lote.CantidadActual=comandoLote.CantidadAnimales;

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
        result.PesoIngreso = lote.PesoIngreso;
        result.IdFinalidad = lote.IdFinalidad;
        result.IdRaza = lote.IdRaza;
        result.EdadMeses = lote.EdadMeses;
        result.CantidadActual=lote.CantidadActual;
        // result.PesoEgreso = (double)lote.PesoEgreso;
        // result.FechaEgreso = (DateOnly)lote.FechaEgreso;
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
        result.PesoIngreso = lote.PesoIngreso;
        result.IdFinalidad = lote.IdFinalidad;
        result.IdEspecie = lote.IdRazaNavigation.IdEspecie;
        result.IdRaza = lote.IdRaza;
        result.EdadMeses = lote.EdadMeses;
        result.CantidadActual = lote.CantidadActual;
        result.FechaEgreso = lote.FechaEgreso;
        result.PesoEgreso = lote.PesoEgreso;

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
                PesoIngreso = l.PesoIngreso,
                Finalidad = l.IdFinalidadNavigation.NombreFinalidad,
                Especie = l.IdRazaNavigation.IdEspecieNavigation.NombreEspecie,
                Raza = l.IdRazaNavigation.NombreRaza,
                EdadMeses = l.EdadMeses,
                PesoPromedioPorAnimalIngreso = l.PesoIngreso / l.CantidadAnimales,
                PesoAproxActual=l.CantidadActual*(l.PesoIngreso / l.CantidadAnimales),
                CantidadActual= l.CantidadActual
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

        // var lote = await _context.Lotes.
        // Include(l => l.Animales).
        // FirstOrDefaultAsync(l => l.IdLote == id);

        var lotePlani = await _context.PlanesAlimentacions
                        .AnyAsync(pa => pa.IdLote == id);
        if (lotePlani)
        {
            resultado.SetError("No se puede eliminar el lote porque ya le ha sido asignado un plan de alimentación");
            resultado.StatusCode = "404";
            return resultado;
            //return BadRequest("No se puede eliminar la dieta porque ya ha sido utilizada en un plan de alimentación");
        }

        // var lote = await _context.Lotes
        // .Include(l => l.Animales)
        // .Include(l => l.PlanesAlimentacions)
        // .FirstOrDefaultAsync(l => l.IdLote == id);

        // Verificar si la dieta existe
        var lote = await _context.Lotes.FindAsync(id);

        if (lote == null)
        {
            resultado.SetError("No se encontró el lote con el ID especificado");
            resultado.StatusCode = "404";
            return resultado;
        }

        // if (lote.PlanesAlimentacions.Any())
        // {
        //     resultado.SetError("No se puede borrar el lote, ya que tiene planes de alimentación asignados");
        //     resultado.StatusCode = "400";  // Cambiado a 400 (Bad Request) ya que la solicitud no es válida debido a la restricción
        //     return resultado;
        // }

        // Eliminar los animales asociados al lote
        // Eliminar los detalles de dieta (alimentos) asociados
        var animalesLote = await _context.Animales
            .Where(al => al.IdLote == id)
            .ToListAsync();
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

        resultado.IdLote = lote.IdLote;
        resultado.StatusCode = "200";
        return Ok(resultado);
    }


    // [HttpPut("api/lote/bajaAnimales/{idlote}")]
    // public async Task<ResultadoUpdateLote> BajaAnimales(int idlote, int cantidad)
    // {
    //     var result = new ResultadoUpdateLote();
    //     var lote = await _context.Lotes.FindAsync(idlote);
    //     if (lote == null)
    //     {
    //         result.SetError("No se encontró el id de lote");
    //         result.StatusCode = "404";
    //         return result;
    //     }

    //     // lote.FechaIngreso = DateOnly.Parse(comandoLote.FechaIngreso);
    //     // lote.CantidadAnimales = comandoLote.CantidadAnimales;
    //     // lote.PesoIngreso = comandoLote.PesoIngreso;
    //     // lote.IdFinalidad = comandoLote.IdFinalidad;
    //     // lote.IdRaza = comandoLote.IdRaza;
    //     // lote.EdadMeses = comandoLote.EdadMeses;
    //     // lote.FechaEgreso = DateOnly.Parse(comandoLote.FechaEgreso);
    //     // lote.PesoEgreso = comandoLote.PesoEgreso;
    //     lote.CantidadActual-= cantidad;



    //     _context.Lotes.Update(lote);

    //     var resultadoUpdate = await _context.SaveChangesAsync();


    //     if (resultadoUpdate < 1)
    //     {
    //         result.SetError("No se pudo actualizar el lote");
    //         result.StatusCode = "404";
    //         return result;
    //     }


    //     result.FechaIngreso = lote.FechaIngreso;
    //     result.CantidadAnimales = lote.CantidadAnimales;
    //     result.PesoIngreso = lote.PesoIngreso;
    //     result.IdFinalidad = lote.IdFinalidad;
    //     result.IdRaza = lote.IdRaza;
    //     result.EdadMeses = lote.EdadMeses;
    //     result.FechaEgreso = (DateOnly)lote.FechaEgreso;
    //     result.StatusCode = "200";
    //     return result;
    // }

    // [HttpPut("api/lote/bajaAnimales/{idlote}")]
    // public async Task BajaAnimales(int idLote, int cantidadAnimalesADarDeBaja)

    // {
    //     // // Obtener los animales del lote
    //     // var animales = await _context.Animales
    //     //     .Where(a => a.IdLote == idLote)
    //     //     .Take(cantidadAnimalesADarDeBaja)
    //     //     .ToListAsync();

    //     // // Dar de baja los animales obtenidos
    //     // _context.Animales.RemoveRange(animales);


    //     // // Guardar los cambios en la base de datos
    //     // await _context.SaveChangesAsync();

    //     // Obtener el lote
    //     var lote = await _context.Lotes.FindAsync(idLote);
    //     if (lote == null)
    //     {
    //         // Manejar el caso en el que no se encuentra el lote
    //         return;
    //     }

    //     // Obtener los animales del lote
    //     var animales = lote.Animales.Take(cantidadAnimalesADarDeBaja).ToList();

    //     // Dar de baja los animales obtenidos
    //     _context.Animales.RemoveRange(animales);

    //     // Actualizar la columna CantidadActual del lote
    //     lote.CantidadActual -= animales.Count;

    //     // Guardar los cambios en la base de datos
    //     await _context.SaveChangesAsync();
    // }

    [HttpPut("api/lote/bajaAnimales/{idLote}/{cantidadAnimalesADarDeBaja}")]
    public async Task<ActionResult<ResultadoUpdateLote>> BajaAnimales(int idLote, int cantidadAnimalesADarDeBaja)
    {
        var resultado = new ResultadoUpdateLote();

        // Obtener el lote
        var lote = await _context.Lotes.Include(l => l.Animales).FirstOrDefaultAsync(l => l.IdLote == idLote);

        if (lote == null)
        {
            resultado.SetError("No se encontró el lote con el ID especificado");
            resultado.StatusCode = "404";
            return resultado;
        }

        // Verificar si la cantidad de animales a dar de baja es mayor que la cantidad actual en el lote
        if (cantidadAnimalesADarDeBaja > lote.CantidadActual)
        {
            resultado.SetError("La cantidad de animales a dar de baja es mayor que la cantidad actual en el lote");
            resultado.StatusCode = "400";
            return resultado;
        }

        // Dar de baja los animales obtenidos
        var animales = lote.Animales.Take(cantidadAnimalesADarDeBaja).ToList();
        _context.Animales.RemoveRange(animales);

        // Actualizar la columna CantidadActual del lote
        lote.CantidadActual -= cantidadAnimalesADarDeBaja;

        // Guardar los cambios en la base de datos
        var resultadoUpdate = await _context.SaveChangesAsync();

        if (resultadoUpdate < 1)
        {
            resultado.SetError("No se pudieron dar de baja los animales del lote");
            resultado.StatusCode = "404";
            return resultado;
        }

        //resultado.IdLote = lote.IdLote;
        resultado.CantidadActual = lote.CantidadActual;
        resultado.StatusCode = "200";
        return Ok(resultado);
    }


    [HttpPut("api/lote/egresarLote/{id}")]
    public async Task<ResultadoUpdateLote> EgresarLote(int id, ComandoEgresoLote comandoLote)
    {
        var result = new ResultadoUpdateLote();
        var lote = await _context.Lotes.FindAsync(id);
        if (lote == null)
        {
            result.SetError("No se encontró el id de lote");
            result.StatusCode = "404";
            return result;
        }

        // Validar si se ingresó una fecha de egreso
        if (!string.IsNullOrEmpty(comandoLote.FechaEgreso))
        {
            // Si hay fecha de egreso, también debe haber peso de egreso
            if (comandoLote.PesoEgreso == 0)
            {
                result.SetError("Si se proporciona una fecha de egreso, también se debe proporcionar un peso de egreso.");
                result.StatusCode = "400"; // Código para Bad Request
                return result;
            }


            lote.FechaIngreso = DateOnly.Parse(comandoLote.FechaIngreso);
            lote.CantidadAnimales = comandoLote.CantidadAnimales;
            lote.PesoIngreso = comandoLote.PesoIngreso;
            lote.IdFinalidad = comandoLote.IdFinalidad;
            lote.IdRaza = comandoLote.IdRaza;
            lote.EdadMeses = comandoLote.EdadMeses;
            lote.FechaEgreso = DateOnly.Parse(comandoLote.FechaEgreso);
            lote.PesoEgreso = comandoLote.PesoEgreso;
            lote.CantidadActual=comandoLote.CantidadAnimales;
            
        }

        else
        {
            result.SetError("El formato de la fecha de egreso no es válido.");
            result.StatusCode = "400"; // Código para Bad Request
            return result;
        }

        // lote.FechaIngreso = lote.FechaIngreso;
        // lote.CantidadAnimales = lote.CantidadAnimales;
        // lote.PesoIngreso = lote.PesoIngreso;
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
        result.PesoIngreso = lote.PesoIngreso;
        result.IdFinalidad = lote.IdFinalidad;
        result.IdRaza = lote.IdRaza;
        result.EdadMeses = lote.EdadMeses;
        result.CantidadActual=lote.CantidadActual;
        result.PesoEgreso = (double)lote.PesoEgreso;
        result.FechaEgreso = (DateOnly)lote.FechaEgreso;
        result.StatusCode = "200";
        return result;
    }

}

