using Frontend.Models;
using Frontend.Resultados.Alimentacion;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Frontend.Controllers;

[ApiController]

public class PlanificacionController : ControllerBase
{
    private readonly FarmFoodNutricionContext _context;

    public PlanificacionController(FarmFoodNutricionContext context)
    {
        _context = context;
    }

    [HttpPost]
    [Route("api/planes/registrarPlani")]

    public async Task<ActionResult<ResultadoAltaPlanAlimentacion>> RegistrarPlanAlimentacion([FromBody] ComandoPlanAlimentacion planAlimentacion)
    {
        var dieta = await _context.Dietas.FindAsync(planAlimentacion.IdDieta);
        var lote = await _context.Lotes.FindAsync(planAlimentacion.IdLote);
        var result = new ResultadoAltaPlanAlimentacion();
        try
        {

            if (dieta == null || lote == null)
            {
                result.SetError("Dieta no encontrado");
                result.StatusCode = "500";
                return Ok(result);
            }

            var PlanAli = new PlanesAlimentacion
            {
                IdLote = planAlimentacion.IdLote,
                IdDieta = planAlimentacion.IdDieta,
                FechaInicio = DateOnly.Parse(planAlimentacion.FechaInicio),
                FechaFin = DateOnly.Parse(planAlimentacion.FechaFin),
                CantToneladaDiaria = planAlimentacion.CantToneladaDiaria
            };

            await _context.AddAsync(PlanAli);
            await _context.SaveChangesAsync();

            result.IdLote = PlanAli.IdLote;
            result.IdDieta = PlanAli.IdDieta;
            result.FechaInicio = PlanAli.FechaInicio;
            result.FechaFin = PlanAli.FechaFin;
            result.ToneladasDispensadas = PlanAli.CantToneladaDiaria;
            return result;
        }
        catch (Exception ex)
        {
            result.SetError($"Ocurrió un error al registrar el plan de alimentación: {ex.Message}");
            return StatusCode(500);
        }
    }

    [HttpGet]
    [Route("api/planes/traerPlaniGetDate")]

    public async Task<ActionResult<ResultadoListPlanAlimentacion>> GetPlanesParaHoy()
    {
        try
        {

            var fechaActual = DateOnly.FromDateTime(DateTime.Now);

            var query = from pa in _context.PlanesAlimentacions
                    join l in _context.Lotes on pa.IdLote equals l.IdLote
                    join r in _context.Razas on l.IdRaza equals r.IdRaza
                    join e in _context.Especies on r.IdEspecie equals e.IdEspecie
                    join d in _context.Dietas on pa.IdDieta equals d.IdDieta
                    where pa.FechaInicio <= fechaActual && pa.FechaFin >= fechaActual
                    && !_context.Alimentaciones.Any(a => a.IdPlan == pa.IdPlan && a.FechaAlimentacion == fechaActual)
                    orderby pa.IdPlan ascending
                        select new ResultadoListPlanAlimentacionItem
                        {
                            IdPlan = pa.IdPlan,
                            IdLote = pa.IdLote,
                            NombreEspecie = e.NombreEspecie,
                            Raza = r.NombreRaza,
                            Cantidad = l.CantidadAnimales,
                            NombreDieta = d.NombreDieta,
                            FechaInicio = pa.FechaInicio,
                            FechaFin = pa.FechaFin,
                            CantPorDiaPorAnimal = (pa.CantToneladaDiaria / l.CantidadAnimales),
                            CantToneladaDiaria = pa.CantToneladaDiaria
                        };

            var resultados = await query.ToListAsync();

            var resultados2 = new ResultadoListPlanAlimentacion
            {
                listaPlanesAlimentacion = resultados
            };

            return resultados2;
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    //FUNCIONA OK PERO LOS VALORES EN NEGATIVO LOS PONE COMO BAJO STOCK CUANDO DEBERIA DECIR SOBRESTOCK
    // [HttpGet("api/reporte/toneladasAlimentoCompra")]
    // public ActionResult<List<AlimentoStockDTO>> GetToneladasAlimentoCompra()
    // {
    //     // Obtener todos los alimentos
    //     var alimentos = _context.Alimentos.ToList();

    //     // Crear una lista para almacenar los datos de cada alimento
    //     var listaAlimentos = new List<AlimentoStockDTO>();

    //     // Recorrer cada alimento
    //     foreach (var alimento in alimentos)
    //     {
    //         // Obtener el stock actual del alimento
    //         decimal stockActual = ObtenerStockAlimentos(alimento.IdAlimento);

    //         // Obtener el stock necesario para cubrir todos los planes de alimentación hasta la última fecha
    //         decimal stockNecesario = ObtenerStockNecesario(alimento.IdAlimento);

    //         // Calcular la cantidad a comprar
    //         decimal cantidadAComprar = stockNecesario - stockActual;

    //         // Calcular el estado
    //         string estado;
    //         if (stockActual == 0 || stockNecesario > stockActual)
    //         {
    //             estado = "SIN STOCK";
    //         }
    //         else if (stockNecesario > 0.3m * stockActual && stockNecesario <= 0.6m * stockActual)
    //         {
    //             estado = "MUY BAJO";
    //         }
    //         else if (stockNecesario <= 0.3m * stockActual)
    //         {
    //             estado = "BAJO";
    //         }
    //         else if (stockActual > 1.1m * stockNecesario)
    //         {
    //             estado = "SOBRESTOCK";
    //             cantidadAComprar = 0; // No se necesita comprar en caso de sobrestock
    //         }
    //         else
    //         {
    //             estado = "OK";
    //         }

    //         // Crear objeto DTO con los datos del alimento
    //         var alimentoDTO = new AlimentoStockDTO
    //         {
    //             IdAlimento = alimento.IdAlimento,
    //             NombreAlimento = alimento.NombreAlimento,
    //             StockActual = stockActual,
    //             StockNecesario = stockNecesario,
    //             CantidadAComprar = cantidadAComprar,
    //             Estado = estado
    //         };

    //         // Agregar el objeto DTO a la lista
    //         listaAlimentos.Add(alimentoDTO);
    //     }

    //     return listaAlimentos;
    // }

    // private decimal ObtenerStockAlimentos(int idAlimento)
    // {
    //     var fechaActual = DateOnly.FromDateTime(DateTime.Now.Date);

    //     // Obtener todos los registros de stock de alimentos para el alimento especificado
    //     var registrosStock = _context.StockAlimentos
    //         .Where(s => s.IdAlimento == idAlimento && s.FechaRegistro <= fechaActual)
    //         .ToList();

    //     // Sumar todas las toneladas de los registros de stock
    //     var stockTotal = registrosStock.Sum(s => s.Toneladas);

    //     // Retornar el stock total
    //     return (decimal)stockTotal;
    // }

    // private decimal ObtenerStockNecesario(int idAlimento)
    // {
    //     // Obtener la última fecha de los planes de alimentación
    //     var ultimaFecha = _context.PlanesAlimentacions.Max(p => p.FechaFin);

    //     // Calcular el stock necesario sumando las toneladas diarias de los planes de alimentación hasta la última fecha
    //     decimal stockNecesario = (decimal)_context.PlanesAlimentacions
    //         .Where(p => p.FechaFin <= ultimaFecha)
    //         .Sum(p => p.CantToneladaDiaria);

    //     return stockNecesario;
    // }

    [HttpGet("api/reporte/toneladasAlimentoCompra")]
    public ActionResult<List<AlimentoStockDTO>> GetToneladasAlimentoCompra()
    {
        // Obtener todos los alimentos
        var alimentos = _context.Alimentos.ToList();

        // Crear una lista para almacenar los datos de cada alimento
        var listaAlimentos = new List<AlimentoStockDTO>();

        // Recorrer cada alimento
        foreach (var alimento in alimentos)
        {
            // Obtener el stock actual del alimento
            decimal stockActual = ObtenerStockAlimentos(alimento.IdAlimento);

            // Obtener el stock necesario para cubrir todos los planes de alimentación hasta la última fecha
            decimal stockNecesario = ObtenerStockNecesario(alimento.IdAlimento);

            // Calcular la cantidad a comprar
            decimal cantidadAComprar = stockNecesario - stockActual;

            // Calcular el estado
            string estado;
            if (stockActual == 0 || stockNecesario > stockActual)
            {
                estado = "SIN STOCK";
            }
            else if (stockActual > 1.1m * stockNecesario)
            {
                estado = "SOBRESTOCK";
                cantidadAComprar = 0; // No se necesita comprar en caso de sobrestock
            }
            else if (cantidadAComprar <= 0.1m * stockNecesario)
            {
                estado = "OK";
            }
            else
            {
                estado = "BAJO";
            }

            // Crear objeto DTO con los datos del alimento
            var alimentoDTO = new AlimentoStockDTO
            {
                IdAlimento = alimento.IdAlimento,
                NombreAlimento = alimento.NombreAlimento,
                StockActual = stockActual,
                StockNecesario = stockNecesario,
                CantidadAComprar = cantidadAComprar >= 0 ? cantidadAComprar : 0,
                Estado = estado
            };

            // Agregar el objeto DTO a la lista
            listaAlimentos.Add(alimentoDTO);
        }

        return listaAlimentos;
    }

    private decimal ObtenerStockAlimentos(int idAlimento)
    {
        var fechaActual = DateOnly.FromDateTime(DateTime.Now.Date);

        // Obtener todos los registros de stock de alimentos para el alimento especificado
        var registrosStock = _context.StockAlimentos
            .Where(s => s.IdAlimento == idAlimento && s.FechaRegistro <= fechaActual)
            .ToList();

        // Sumar todas las toneladas de los registros de stock
        var stockTotal = registrosStock.Sum(s => s.Toneladas);

        // Retornar el stock total
        return (decimal)stockTotal;
    }

    private decimal ObtenerStockNecesario(int idAlimento)
    {
        // Obtener la última fecha de los planes de alimentación
        var ultimaFecha = _context.PlanesAlimentacions.Max(p => p.FechaFin);

        // Calcular el stock necesario sumando las toneladas diarias de los planes de alimentación hasta la última fecha
        decimal stockNecesario = (decimal)_context.PlanesAlimentacions
            .Where(p => p.FechaFin <= ultimaFecha)
            .Sum(p => p.CantToneladaDiaria);

        return stockNecesario;
    }



}


