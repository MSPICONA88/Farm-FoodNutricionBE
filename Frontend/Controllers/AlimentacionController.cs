using System.Runtime.Intrinsics.X86;
using Frontend.Comandos.Alimentacion;
using Frontend.Models;
using Frontend.Resultados.Alimentacion;
using Frontend.Resultados.Alimentos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Frontend.Controllers;

[ApiController]

public class AlimentacionController : ControllerBase
{
    private readonly FarmFoodNutricionContext _context;

    public AlimentacionController(FarmFoodNutricionContext context)
    {
        _context = context;
    }

    // [HttpPost]
    // [Route("api/alimentacion/registrar")]
    // public async Task<IActionResult> RegistrarAlimentacion([FromBody] ComandoAlimentacion alimentacion)
    // {
    //     try
    //     {
    //         // Verificar si el plan existe
    //         var plan = await _context.PlanesAlimentacions
    //             .Include(p => p.IdDietaNavigation)
    //                 .ThenInclude(d => d.AlimentosxDieta)
    //                     .ThenInclude(ad => ad.IdAlimentoNavigation)
    //             .FirstOrDefaultAsync(p => p.IdPlan == alimentacion.IdPlan);

    //         if (plan == null)
    //         {
    //             return NotFound("El plan de alimentación no existe");
    //         }
    //         var dieta = plan.IdDietaNavigation;

    //         if (dieta.AlimentosxDieta == null || dieta.AlimentosxDieta.Count == 0)
    //         {
    //             throw new InvalidOperationException("La dieta no tiene alimentos cargados");
    //         }

    //         // Verificar si la alimentación ya existe
    //         var existente = await _context.Alimentaciones.FirstOrDefaultAsync(a =>
    //             a.IdPlan == alimentacion.IdPlan &&
    //             a.FechaAlimentacion == DateOnly.Parse(alimentacion.FechaAlimentacion));

    //         if (existente != null)
    //         {
    //             return BadRequest("La alimentación ya ha sido registrada");
    //         }

    //         // Descontar los alimentos de la dieta y registrar el movimiento de stock
    //         foreach (var alimentoPorDieta in dieta.AlimentosxDieta)
    //         {
    //             var alimento = alimentoPorDieta.IdAlimentoNavigation;
    //             // var cantidadDescontar = (alimentoPorDieta.Porcentaje) * (decimal)alimentacion.ToneladasDispensadas;
    //             var cantidadDescontar = -(alimentoPorDieta.Porcentaje / 100m) * (decimal)alimentacion.ToneladasDispensadas;

    //             // Verificar si hay suficiente stock disponible
    //             var stock = await _context.StockAlimentos.FirstOrDefaultAsync(s => s.IdAlimento == alimento.IdAlimento);
    //             if (stock == null)
    //             {
    //                 return BadRequest($"No hay suficiente stock disponible del alimento: {alimento.NombreAlimento}");
    //             }

    //             // Registrar el movimiento de stock en la tabla StockAlimentos
    //             var nuevoMovimiento = new StockAlimento
    //             {
    //                 IdAlimento = alimento.IdAlimento,
    //                 FechaRegistro = DateOnly.Parse(alimentacion.FechaAlimentacion),
    //                 Toneladas = (double)cantidadDescontar,
    //                 PrecioTonelada = 0,
    //                 IdTipoMovimiento = 5 // Reemplazar con el ID correcto del tipo de movimiento
    //             };

    //             _context.StockAlimentos.Add(nuevoMovimiento);

    //             // Actualizar el stock del alimento en la base de datos
    //             stock.Toneladas -= (double)cantidadDescontar;
    //             _context.Entry(stock).State = EntityState.Modified;
    //         }

    //         /// Crear la alimentación
    //         var nuevaAlimentacion = new Alimentacione
    //         {
    //             IdPlan = alimentacion.IdPlan,
    //             FechaAlimentacion = DateOnly.Parse(alimentacion.FechaAlimentacion),
    //             ToneladasDispensadas = alimentacion.ToneladasDispensadas
    //         };

    //         // Guardar la alimentación en la base de datos
    //         _context.Alimentaciones.Add(nuevaAlimentacion);

    //         await _context.SaveChangesAsync();

    //         return Ok("Alimentación registrada con éxito");
    //     }
    //     catch (Exception ex)
    //     {
    //         return StatusCode(500, $"Error al registrar la alimentación: {ex.Message}");
    //     }
    // }

    [HttpPost]
    [Route("api/alimentacion/registrar")]
    public async Task<ActionResult<ResultadoRegistrarAlimentacion>> RegistrarAlimentacion([FromBody] ComandoAlimentacion alimentacion)
    {
        var result = new ResultadoRegistrarAlimentacion();
        try
        {
            // Verificar si el plan existe
            var plan = await _context.PlanesAlimentacions
                .Include(p => p.IdDietaNavigation)
                    .ThenInclude(d => d.AlimentosxDieta)
                        .ThenInclude(ad => ad.IdAlimentoNavigation)
                .FirstOrDefaultAsync(p => p.IdPlan == alimentacion.IdPlan);

            if (plan == null)
            {
                result.SetError("El plan de alimentación no existe");
                return Ok(result);
            }
            var dieta = plan.IdDietaNavigation;

            if (dieta.AlimentosxDieta == null || dieta.AlimentosxDieta.Count == 0)
            {
                result.SetError("La dieta no tiene alimentos cargados");
                return Ok(result);

                // throw new InvalidOperationException("La dieta no tiene alimentos cargados");
            }

            // Verificar si la alimentación ya existe
            var existente = await _context.Alimentaciones.FirstOrDefaultAsync(a =>
                a.IdPlan == alimentacion.IdPlan &&
                a.FechaAlimentacion == DateOnly.Parse(alimentacion.FechaAlimentacion));

            if (existente != null)
            {
                result.SetError("La alimentación ya ha sido registrada para el dia de hoy");
                return Ok(result);
                // return BadRequest("La alimentación ya ha sido registrada");
            }

            // Descontar los alimentos de la dieta y registrar el movimiento de stock
            foreach (var alimentoPorDieta in dieta.AlimentosxDieta)
            {
                var alimento = alimentoPorDieta.IdAlimentoNavigation;
                // var cantidadDescontar = (alimentoPorDieta.Porcentaje) * (decimal)alimentacion.ToneladasDispensadas;
                var cantidadDescontar = -(alimentoPorDieta.Porcentaje / 100m) * (decimal)alimentacion.ToneladasDispensadas;

                // Verificar si hay suficiente stock disponible
                var stock = await _context.StockAlimentos.FirstOrDefaultAsync(s => s.IdAlimento == alimento.IdAlimento);
                if (stock == null)
                {
                    result.SetError($"No hay suficiente stock disponible del alimento: {alimento.NombreAlimento}");
                    return Ok(result);
                    // return BadRequest($"No hay suficiente stock disponible del alimento: {alimento.NombreAlimento}");
                }

                // Registrar el movimiento de stock en la tabla StockAlimentos
                var nuevoMovimiento = new StockAlimento
                {
                    IdAlimento = alimento.IdAlimento,
                    FechaRegistro = DateOnly.Parse(alimentacion.FechaAlimentacion),
                    Toneladas = (double)cantidadDescontar,
                    PrecioTonelada = 0,
                    IdTipoMovimiento = 5 // Reemplazar con el ID correcto del tipo de movimiento
                };

                _context.StockAlimentos.Add(nuevoMovimiento);

                // Actualizar el stock del alimento en la base de datos
                stock.Toneladas -= (double)cantidadDescontar;
                _context.Entry(stock).State = EntityState.Modified;
            }

            /// Crear la alimentación
            var nuevaAlimentacion = new Alimentacione
            {
                IdPlan = alimentacion.IdPlan,
                FechaAlimentacion = DateOnly.Parse(alimentacion.FechaAlimentacion),
                ToneladasDispensadas = alimentacion.ToneladasDispensadas
            };

            // Guardar la alimentación en la base de datos
            _context.Alimentaciones.Add(nuevaAlimentacion);

            await _context.SaveChangesAsync();

            result.IdPlan=nuevaAlimentacion.IdPlan;
            //result.FechaAlimentacion= DateOnly.Parse(nuevaAlimentacion.FechaAlimentacion);
            result.ToneladasDispensadas= (int)nuevaAlimentacion.ToneladasDispensadas;
            result.StatusCode="200";
            return Ok(result);

            // return Ok("Alimentación registrada con éxito");
        }
        catch (Exception ex)
        {
            result.StatusCode="500";
            result.SetError($"Error al registrar la alimentación: {ex.Message}");
            return Ok(result);
            // return StatusCode(500, $"Error al registrar la alimentación: {ex.Message}");
        }
    }



}
