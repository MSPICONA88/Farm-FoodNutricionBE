using Frontend.Comandos.Dietas;
using Frontend.Comandos.Usuarios;
using Frontend.Models;
using Frontend.Resultados.Alimentos;
using Frontend.Resultados.Usuarios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Frontend.Controllers;

[ApiController]

public class AlimentoController : ControllerBase
{
    private readonly FarmFoodNutricionContext _context;

    public AlimentoController(FarmFoodNutricionContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Route("api/alimento/traerTodos")]

    public async Task<ActionResult<ResultadoListAlimentos>> GetAlimentos()

    {
        try
        {
            var result = new ResultadoListAlimentos();
            var alimentos = await _context.Alimentos.ToListAsync();
            if (alimentos != null)
            {
                foreach (var ali in alimentos)
                {
                    var resultAux = new ResultadoListAlimentosItem
                    {
                        IdAlimento= ali.IdAlimento,
                        NombreAlimento = ali.NombreAlimento

                    };
                    result.listaAlimentos.Add(resultAux);
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
            return BadRequest("Error al obtener los alimentos");
        }
    }

    [HttpPost]
    [Route("api/alimento/alta")]

    public async Task<ActionResult<ResultadoAltaAlimento>> AltaAlimento([FromBody] Alimento alimento)
    {

        var result = new ResultadoAltaAlimento();

        var alimentoResult = new Alimento
        {
            NombreAlimento = alimento.NombreAlimento,

        };

        await _context.AddAsync(alimentoResult);
        await _context.SaveChangesAsync();

        result.NombreAlimento = alimento.NombreAlimento;

        result.StatusCode = "200";
        return Ok(result);
    }


    [HttpPost]
    [Route("api/alimento/altaConVerif")]
    public async Task<ActionResult<ResultadoAltaAlimento>> AltaAlimentoVerif([FromBody] ComandoAlimento comando)
    {
        var alimentoExistente = await _context.Alimentos.FirstOrDefaultAsync(a => a.NombreAlimento == comando.NombreAlimento);
        var result = new ResultadoAltaAlimento();

        if (alimentoExistente != null)
        {
            // Si el nombre del alimento ya existe, retornar un mensaje de error
            result.SetError("El nombre del alimento ya existe");
            //result.StatusCode("500");
            return Ok(result);
            //return BadRequest("El nombre del alimento ya existe");
        }

        var alimentoResult = new Alimento
        {
            IdAlimento = comando.IdAlimento,
            NombreAlimento = comando.NombreAlimento

        };

        await _context.AddAsync(alimentoResult);
        await _context.SaveChangesAsync();

        result.NombreAlimento = alimentoResult.NombreAlimento;

        result.StatusCode = "200";
        return Ok(result);
    }

    [HttpPost]
    [Route("api/alimento/aliPorDieta")]
    public async Task<ActionResult<ResultadoAliPorDieta>> AltaDieta([FromBody] ComandoAliPorDieta alixdieta)
    {
        var idDieta = await _context.Dietas.FirstOrDefaultAsync(a => a.IdDieta == alixdieta.IdDieta);
        var idalimento= await _context.Alimentos.FirstOrDefaultAsync(a => a.IdAlimento == alixdieta.IdAlimento);
        var result = new ResultadoAliPorDieta();

        var dietaResult = new AlimentosxDietum
        {
            //IdDieta = dieta.IdDieta,
            IdDieta = alixdieta.IdDieta,
            IdAlimento= alixdieta.IdAlimento,
            Porcentaje= (int)alixdieta.Porcentaje
            
        };

        await _context.AddAsync(dietaResult);
        await _context.SaveChangesAsync();

        result.IdDieta = dietaResult.IdDieta;
        result.IdAlimento= dietaResult.IdAlimento;
        result.Porcentaje= dietaResult.Porcentaje;

        result.StatusCode = "200";
        return Ok(result);
    }
}