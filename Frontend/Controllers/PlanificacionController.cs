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

    [HttpGet]
    [Route("api/planes/traerPlanificacion")]

    public async Task<ActionResult<ResultadoListPlanAlimentacion>> GetAllPlanes()
    {
        try
        {
            var query = from pa in _context.PlanesAlimentacions
                        join l in _context.Lotes on pa.IdLote equals l.IdLote
                        join r in _context.Razas on l.IdRaza equals r.IdRaza
                        join e in _context.Especies on r.IdEspecie equals e.IdEspecie
                        join d in _context.Dietas on pa.IdDieta equals d.IdDieta
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

    [HttpGet]
    [Route("api/planes/traerPlaniPorId/{idPlan}")]
    public async Task<ActionResult<ResultadoPlanPorId>> GetPlanPorId(int idPlan)
    {
        try
        {
            var resultado = await (from pa in _context.PlanesAlimentacions
                                   join l in _context.Lotes on pa.IdLote equals l.IdLote
                                   join r in _context.Razas on l.IdRaza equals r.IdRaza
                                   join e in _context.Especies on r.IdEspecie equals e.IdEspecie
                                   join d in _context.Dietas on pa.IdDieta equals d.IdDieta
                                   where pa.IdPlan == idPlan
                                   select new ResultadoPlanPorId
                                   {
                                       IdPlan = pa.IdPlan,
                                       IdLote = pa.IdLote,
                                       FechaInicio = pa.FechaInicio,
                                       FechaFin = pa.FechaFin,
                                       CantToneladaDiaria = pa.CantToneladaDiaria
                                   })
                                   .FirstOrDefaultAsync();

            if (resultado == null)
            {
                return NotFound(); // 404 Not Found si el plan no existe
            }

            return resultado;
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut("api/planes/editarPlaniPorId/{idPlan}")]
    public async Task<ResultadoUpdatePlanAlimentacion> EditarPlani(int idPlan, ComandoPlanAlimentacion comandoPlani)
    {
        var result = new ResultadoUpdatePlanAlimentacion();
        var plani = await _context.PlanesAlimentacions.FindAsync(idPlan);
        if (plani == null)
        {
            result.SetError("No se encontró el id de plani");
            result.StatusCode = "404";
            return result;
        }

        // Validar si se ingresó una fecha de egreso



        plani.IdLote = comandoPlani.IdLote;
        plani.IdDieta = comandoPlani.IdDieta;
        plani.FechaInicio = DateOnly.Parse(comandoPlani.FechaInicio);
        plani.FechaFin = DateOnly.Parse(comandoPlani.FechaFin);
        plani.CantToneladaDiaria = comandoPlani.CantToneladaDiaria;


        _context.PlanesAlimentacions.Update(plani);

        var resultadoUpdate = await _context.SaveChangesAsync();


        if (resultadoUpdate < 1)
        {
            result.SetError("No se pudo actualizar la planificacion");
            result.StatusCode = "404";
            return result;
        }


        result.IdLote = plani.IdLote;
        result.IdDieta = plani.IdDieta;
        result.FechaInicio = plani.FechaInicio;
        result.FechaFin = plani.FechaFin;
        result.CantToneladaDiaria = plani.CantToneladaDiaria;

        // result.PesoEgreso = (double)lote.PesoEgreso;
        // result.FechaEgreso = (DateOnly)lote.FechaEgreso;
        result.StatusCode = "200";
        return result;
    }

    [HttpDelete("api/planes/borrarPlani/{id}")]
    public async Task<ActionResult<ResultadoBorrarPlani>> BorrarPlani(int id)
    {
        var resultado = new ResultadoBorrarPlani();

        // Verificar si la dieta existe
        var plan = await _context.PlanesAlimentacions.FindAsync(id);

        if (plan == null)
        {
            resultado.SetError("No se encontró el plan de alimentación con el ID especificado");
            resultado.StatusCode = "404";
            return resultado;
        }

        // Verificar si hay alimentaciones asociadas al plan
        if (_context.Alimentaciones.Any(a => a.IdPlan == id))
        {
            resultado.SetError("No se puede borrar el plan de alimentación, ya que ha sido utilizado en alimentaciones");
            resultado.StatusCode = "400";  // Cambiado a 400 (Bad Request) ya que la solicitud no es válida debido a la restricción
            return resultado;
        }

        // Eliminar el plan de alimentación
        _context.PlanesAlimentacions.Remove(plan);

        var resultadoDelete = await _context.SaveChangesAsync();

        if (resultadoDelete < 1)
        {
            resultado.SetError("No se pudo borrar el plan de alimentación");
            resultado.StatusCode = "404";
            return resultado;
        }

        resultado.IdPlani = plan.IdLote;
        resultado.StatusCode = "200";
        return Ok(resultado);
    }

    [HttpGet("api/reporte/toneladasAlimentoCompra")]
    public ActionResult<List<AlimentoStockDTO>> GetToneladasAlimentoCompra()
    {
        // Obtener todos los alimentos
        var alimentos = _context.Alimentos
        .OrderBy(a => a.NombreAlimento)
        .ToList();

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
            decimal porcentajeFaltante = (stockNecesario != 0) ? (stockNecesario - stockActual) / stockNecesario * 100 : 0;

            //decimal porcentajeSobrante = (stockActual-stockNecesario) / stockNecesario * 100;
                if (stockActual < stockNecesario)
                {
                    if (porcentajeFaltante <= 10)
                    {
                        estado = "BAJO";
                    }
                    else
                    {
                        estado = "SIN STOCK";
                    }
                }
                else
                {
                    if ((porcentajeFaltante >= -10 && cantidadAComprar > 0) || stockNecesario == 0)
                    {
                        estado = "OK";
                    }
                    else
                    {
                        estado = "SOBRESTOCK";
                        cantidadAComprar = 0; // No se necesita comprar en caso de sobrestock
                    }
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
        DateOnly fechaActual = DateOnly.FromDateTime(DateTime.Now.Date);
        var cantNecesariaFinal = 0.0;
        var planesAlimentacionQuery = from plan in _context.PlanesAlimentacions
                                      join dieta in _context.Dietas on plan.IdDieta equals dieta.IdDieta
                                      join alimentosDieta in _context.AlimentosxDieta on dieta.IdDieta equals alimentosDieta.IdDieta
                                      where alimentosDieta.IdAlimento == idAlimento  // Reemplaza el 4 con el idAlimento que estás buscando
                                      orderby plan.IdPlan
                                      select new
                                      {
                                          Plan = plan,
                                          Dieta = dieta,
                                          AlimentosDieta = alimentosDieta
                                      };

        var planesAlimentacion = planesAlimentacionQuery.ToList();

        foreach (var plan in planesAlimentacion)
        {
            DateOnly fechaInicio = plan.Plan.FechaInicio;
            DateOnly fechaFin = plan.Plan.FechaFin;

            // Calcular la cantidad de días según la lógica especificada
            int cantidadDias = (fechaInicio <= fechaActual && fechaFin > fechaActual)
                ? (int)((fechaFin.DayNumber - fechaActual.DayNumber)+1)
                : (fechaFin < fechaActual)
                    ? 0 //vencido
                    : (int)(fechaFin.DayNumber - fechaInicio.DayNumber)+1;


            // Calcular la cantidad de alimento necesaria
            decimal cantidadAlimentoNecesaria = cantidadDias * (decimal)plan.Plan.CantToneladaDiaria * (plan.AlimentosDieta.Porcentaje / 100.0m);
            cantNecesariaFinal += (double)cantidadAlimentoNecesaria;
            // Puedes hacer lo que necesites con la cantidad de alimento calculada
            Console.WriteLine($"Plan {plan.Plan.IdPlan}: {cantidadAlimentoNecesaria} toneladas de alimento necesarias");
        }

        return (decimal)cantNecesariaFinal;

    }

}
