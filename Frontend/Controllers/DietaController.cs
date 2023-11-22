using System.Security.AccessControl;
using System.Transactions;
using Frontend.Comandos.Dietas;
using Frontend.Models;
using Frontend.Resultados.Dietas;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Frontend.Resultados.Dietas.ResultadoListAlimentosPorIdDieta;
using static Frontend.Resultados.Dietas.ResultadoListNutrientesPorIdDieta;

namespace Frontend.Controllers;

[ApiController]

public class DietaController : ControllerBase
{

    private readonly FarmFoodNutricionContext _context;

    public DietaController(FarmFoodNutricionContext context)
    {
        _context = context;
    }

    // [HttpPost]
    // [Route("api/dieta/altaDieta")]
    // public async Task<ActionResult<ResultadoAltaDieta>> AltaDieta([FromBody] ComandoDieta dieta)
    // {
    //     var dietaExistente = await _context.Dietas.FirstOrDefaultAsync(a => a.NombreDieta == dieta.NombreDieta);
    //     var result = new ResultadoAltaDieta();

    //     if (dietaExistente != null)
    //     {
    //         // Si el nombre de la dieta ya existe, retornar un mensaje de error
    //         result.SetError("El nombre de la dieta ya existe");
    //         //result.StatusCode("500");
    //         return Ok(result);

    //     }

    //     var dietaResult = new Dieta
    //     {
    //         //IdDieta = dieta.IdDieta,
    //         NombreDieta = dieta.NombreDieta,
    //         FechaCreacion= DateOnly.Parse(dieta.FechaCreacion),
    //         Observacion= dieta.Observacion

    //     };

    //     await _context.AddAsync(dietaResult);
    //     await _context.SaveChangesAsync();

    //     result.IdDieta = dietaResult.IdDieta;
    //     result.NombreDieta= dietaResult.NombreDieta;
    //     result.FechaCreacion= dietaResult.FechaCreacion;
    //     result.Observacion= dieta.Observacion;

    //     result.StatusCode = "200";
    //     return Ok(result);
    // }

    [HttpPost]
    [Route("api/dieta/altaDieta")]
    public async Task<ActionResult<ResultadoAltaDieta>> AltaDieta([FromBody] DietaDTO dietaDTO)
    {
        var result = new ResultadoAltaDieta();
        using (var transaction = _context.Database.BeginTransaction())
        {
            try
            {
                // Validar si el ID de Alimento es correcto o nulo
                var alimentoIds = dietaDTO.Alimentos.Select(a => a.IdAlimento);
                var alimentoIdsInvalidos = alimentoIds.Except(_context.Alimentos.Select(a => a.IdAlimento));
                if (alimentoIdsInvalidos.Any())
                {
                    result.SetError("Alimento Inválido");
                    result.StatusCode = "500";
                    return Ok(result);
                    // return BadRequest($"ID(s) de alimento inválido(s): {string.Join(", ", alimentoIdsInvalidos)}");
                }

                // Validar si la suma de los porcentajes es igual a 1
                var sumaPorcentajes = dietaDTO.Alimentos.Sum(a => a.Porcentaje);
                if (Math.Abs(sumaPorcentajes - 100) > double.Epsilon)
                {
                    result.SetError("La suma de los porcentajes de los alimentos debe ser igual a 100");
                    result.StatusCode = "500";
                    return Ok(result);
                    // return BadRequest("La suma de los porcentajes de los alimentos debe ser igual a 1");
                }


                // Crear la dieta
                var dieta = new Dieta
                {
                    NombreDieta = dietaDTO.NombreDieta,
                    FechaCreacion = DateOnly.Parse(dietaDTO.FechaCreacion),
                    Observacion = dietaDTO.Observacion
                };

                await _context.AddAsync(dieta);
                await _context.SaveChangesAsync();

                // Crear los alimentos por dieta
                var alimentosPorDieta = dietaDTO.Alimentos.Select(alimentoDto => new AlimentosxDietum
                {
                    IdAlimento = alimentoDto.IdAlimento,
                    IdDieta = dieta.IdDieta,
                    Porcentaje = (int)alimentoDto.Porcentaje
                }).ToList();

                await _context.AddRangeAsync(alimentosPorDieta);
                await _context.SaveChangesAsync();

                // Commit de la transacción
                await transaction.CommitAsync();

                result.NombreDieta = dieta.NombreDieta;
                result.FechaCreacion = dieta.FechaCreacion;
                result.Observacion = dieta.Observacion;
                result.StatusCode = "200";
                return Ok(result);

                // return Ok("Dieta creada exitosamente");
            }
            catch (Exception ex)
            {
                // Rollback de la transacción en caso de error
                await transaction.RollbackAsync();

                result.SetError("Error al crear la dieta");
                result.StatusCode = "500";
                return Ok(result);
                // return BadRequest($"Error al crear la dieta: {ex.Message}");
            }
        }
    }


    // [HttpGet("api/dieta/traerDietas")]
    // public async Task<ActionResult<ResultadoListDietas>> GetDietas()
    // {
    //     try
    //     {
    //         var result = new ResultadoListDietas();
    //         var dietas = await _context.Dietas.ToListAsync();

    //         if (dietas != null)
    //         {
    //             foreach (var dieta in dietas)
    //             {
    //                 var resultAux = new ResultadoListDietasItem
    //                 {
    //                     IdDieta = dieta.IdDieta,
    //                     NombreDieta = dieta.NombreDieta,
    //                     FechaCreacion = dieta.FechaCreacion,
    //                     Observacion = dieta.Observacion
    //                 };

    //                 result.listaDietas.Add(resultAux);
    //                 result.StatusCode = "200";
    //             }

    //             return Ok(result);
    //         }
    //         else
    //         {
    //             return Ok(result);
    //         }
    //     }
    //     catch (Exception e)
    //     {
    //         return BadRequest("Error al obtener las dietas");
    //     }
    // }

    [HttpGet("api/dieta/traerDietasDet")]
    public async Task<ActionResult<ResultadoListDietasConAlimentos>> GetDietasConAlimentos()
    {
        try
        {
            var resultado = new ResultadoListDietasConAlimentos();

            var dietas = await _context.Dietas.ToListAsync();

            foreach (var dieta in dietas)
            {
                var resultadoDieta = new ResultadoListDietasConAlimentosItem
                {
                    IdDieta = dieta.IdDieta,
                    NombreDieta = dieta.NombreDieta,
                    FechaCreacion = dieta.FechaCreacion.ToString(),
                    Observacion = dieta.Observacion,
                    Alimentos = new List<AlimentoDetalleDTO>()
                };

                var alimentosxDieta = await _context.AlimentosxDieta
                    .Where(ad => ad.IdDieta == dieta.IdDieta)
                    .ToListAsync();

                foreach (var ad in alimentosxDieta)
                {
                    var alimento = await _context.Alimentos.FindAsync(ad.IdAlimento);

                    if (alimento != null)
                    {
                        var alimentoDTO = new AlimentoDetalleDTO
                        {
                            IdAlimento = alimento.IdAlimento,
                            NombreAlimento = alimento.NombreAlimento,
                            Porcentaje = ad.Porcentaje
                        };

                        resultadoDieta.Alimentos.Add(alimentoDTO);
                    }
                }

                resultado.listaDietas.Add(resultadoDieta);
            }

            return Ok(resultado);
        }
        catch (Exception e)
        {
            return BadRequest("Error al obtener las dietas con alimentos");
        }
    }

    [HttpGet("api/dieta/traerNutrientesPorId{idDieta}")]
    public async Task<ActionResult<ResultadoListNutrientesPorIdDieta>> GetNutrientesPorIdDieta(int idDieta)
    {
        try
        {
            var resultadoConsulta = await _context.AlimentosxDieta
                .Join(_context.Alimentos, ad => ad.IdAlimento, a => a.IdAlimento, (ad, a) => new { AlimentosxDieta = ad, Alimento = a })
                .Join(_context.Dietas, join1 => join1.AlimentosxDieta.IdDieta, d => d.IdDieta, (join1, d) => new { AlimentosxDieta = join1.AlimentosxDieta, Alimento = join1.Alimento, Dieta = d })
                .Join(_context.NutrientesxAlimentos, join2 => join2.Alimento.IdAlimento, na => na.IdAlimento, (join2, na) => new { AlimentosxDieta = join2.AlimentosxDieta, Alimento = join2.Alimento, Dieta = join2.Dieta, NutrientesxAlimento = na })
                .Join(_context.Nutrientes, join3 => join3.NutrientesxAlimento.IdNutriente, n => n.IdNutriente, (join3, n) => new { AlimentosxDieta = join3.AlimentosxDieta, Alimento = join3.Alimento, Dieta = join3.Dieta, NutrientesxAlimento = join3.NutrientesxAlimento, Nutriente = n })
                .Where(result => result.AlimentosxDieta.IdDieta == idDieta)
                .GroupBy(result => result.Nutriente.NombreNutriente)
                .Select(group => new ResultadoListNutrientesPorIdDietaItem
                {
                    NombreNutriente = group.Key,
                    Porcentaje = group.Sum(result => result.NutrientesxAlimento.Porcentaje)
                })
                .ToListAsync();

            var resultado = new ResultadoListNutrientesPorIdDieta
            {
                listaNutrientes = resultadoConsulta,
                StatusCode = "200"
            };

            return Ok(resultado);
        }
        catch (Exception e)
        {
            return BadRequest("Error al realizar la consulta");
        }
    }

    [HttpGet("api/dieta/traerAlimentosPorId{idDieta}")]
    public async Task<ActionResult<ResultadoListAlimentosPorIdDieta>> GetAlimentosPorIdDieta(int idDieta)
    {
        try
        {
            var resultadoConsulta = await _context.AlimentosxDieta
                .Join(_context.Alimentos, ad => ad.IdAlimento, a => a.IdAlimento, (ad, a) => new { AlimentosxDieta = ad, Alimento = a })
                .Join(_context.Dietas, join1 => join1.AlimentosxDieta.IdDieta, d => d.IdDieta, (join1, d) => new { AlimentosxDieta = join1.AlimentosxDieta, Alimento = join1.Alimento, Dieta = d })
                .Where(result => result.AlimentosxDieta.IdDieta == idDieta)
                .Select(result => new ResultadoListAlimentosPorIdDietaItem
                {
                    IdDieta = result.Dieta.IdDieta,
                    NombreDieta = result.Dieta.NombreDieta,
                    NombreAlimento = result.Alimento.NombreAlimento,
                    Porcentaje = result.AlimentosxDieta.Porcentaje
                })
                .ToListAsync();

            var resultado = new ResultadoListAlimentosPorIdDieta
            {
                listaAlimentos = resultadoConsulta,
                StatusCode = "200"
            };

            return Ok(resultado);
        }
        catch (Exception e)
        {
            return BadRequest("Error al realizar la consulta");
        }
    }

    [HttpPut("api/dieta/update{idDieta}")]
    public async Task<IActionResult> UpdateDieta(int idDieta, [FromBody] ComandoDietaDetalleDTOU dietaDetalleDTO)
    {
        try
        {
            // Verificar si la dieta existe
            var dieta = await _context.Dietas.Include(d => d.AlimentosxDieta).FirstOrDefaultAsync(d => d.IdDieta == idDieta);
            if (dieta == null)
            {
                return NotFound();
            }

            // Actualizar la información de la dieta
            dieta.NombreDieta = dietaDetalleDTO.NombreDieta;
            dieta.FechaCreacion = DateOnly.Parse(dietaDetalleDTO.FechaCreacion);
            dieta.Observacion = dietaDetalleDTO.Observacion;

            // Validar que no se ingresen alimentos duplicados
            var alimentoIds = dietaDetalleDTO.Alimentos.Select(a => a.IdAlimento);
            var alimentoIdsDuplicados = alimentoIds.GroupBy(x => x).Where(g => g.Count() > 1).Select(g => g.Key).ToList();
            if (alimentoIdsDuplicados.Any())
            {
                return BadRequest($"No se pueden ingresar alimentos duplicados: {string.Join(", ", alimentoIdsDuplicados)}");
            }

            // Actualizar el detalle de alimentos
            var alimentosPorDieta = new List<AlimentosxDietum>();
            foreach (var alimentoDetalle in dietaDetalleDTO.Alimentos)
            {
                var alimentoPorDieta = dieta.AlimentosxDieta.FirstOrDefault(ad => ad.IdAlimento == alimentoDetalle.IdAlimento);
                if (alimentoPorDieta != null)
                {
                    // Actualizar el porcentaje del alimento existente
                    alimentoPorDieta.Porcentaje = (int)alimentoDetalle.Porcentaje;
                }
                else
                {
                    // Agregar un nuevo alimento al detalle
                    alimentoPorDieta = new AlimentosxDietum
                    {
                        IdAlimento = alimentoDetalle.IdAlimento,
                        //NombreAlimento = alimentoDetalle.NombreAlimento,
                        Porcentaje = (int)alimentoDetalle.Porcentaje
                    };
                    dieta.AlimentosxDieta.Add(alimentoPorDieta);
                }
                alimentosPorDieta.Add(alimentoPorDieta);
            }
            // Validar la suma de los porcentajes de los alimentos
            var sumaPorcentajes = alimentosPorDieta.Sum(a => a.Porcentaje);
            if (sumaPorcentajes > 100)
            {
                return BadRequest("La suma de los porcentajes de los alimentos no puede ser mayor a 100");
            }

            // Eliminar los alimentos que no se incluyeron en el detalle
            var alimentosAEliminar = dieta.AlimentosxDieta.Where(ad => !alimentosPorDieta.Contains(ad)).ToList();
            foreach (var alimentoAEliminar in alimentosAEliminar)
            {
                _context.AlimentosxDieta.Remove(alimentoAEliminar);
            }

            // Guardar los cambios en la base de datos
            await _context.SaveChangesAsync();

            // Crear el objeto ResultadoUpdateDieta con los datos de la dieta modificada
            var resultado = new ResultadoUpdateDieta
            {
                NombreDieta = dietaDetalleDTO.NombreDieta,
                FechaCreacion = dietaDetalleDTO.FechaCreacion,
                Observacion = dietaDetalleDTO.Observacion,
                Alimentos = dietaDetalleDTO.Alimentos.Select(alimento =>
                new AlimentoDietaDTO2
                {
                    IdAlimento = alimento.IdAlimento,
                    Porcentaje = alimento.Porcentaje
                }).ToList(),
                StatusCode = "200"
            };


            return Ok(resultado); // Devuelve una respuesta con el código de estado 200 OK y el objeto resultado
        }

        catch (Exception e)
        {

            return BadRequest("Error al modificar la dieta");
        }
    }


    [HttpGet("api/dieta/getDietaDet{idDieta}")]
    public async Task<ActionResult<ComandoDietaDetalleDTO>> GetDieta(int idDieta)
    {
        try
        {
            var dieta = await _context.Dietas.Include(d => d.AlimentosxDieta)
                                              .ThenInclude(ad => ad.IdAlimentoNavigation)
                                             .FirstOrDefaultAsync(d => d.IdDieta == idDieta);

            if (dieta == null)
            {
                return NotFound();
            }

            var dietaDetalle = new ComandoDietaDetalleDTO
            {
                IdDieta = dieta.IdDieta,
                NombreDieta = dieta.NombreDieta,
                FechaCreacion = dieta.FechaCreacion.ToString(),
                Observacion = dieta.Observacion,
                Alimentos = dieta.AlimentosxDieta.Select(ad => new AlimentoDetalleDTO
                {
                    IdAlimento = ad.IdAlimentoNavigation.IdAlimento,
                    NombreAlimento = ad.IdAlimentoNavigation.NombreAlimento,
                    Porcentaje = ad.Porcentaje
                }).ToList()
            };

            return Ok(dietaDetalle);
        }
        catch (Exception e)
        {
            return BadRequest("Error al obtener la dieta");
        }
    }



}



