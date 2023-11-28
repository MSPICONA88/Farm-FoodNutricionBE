using System.Xml.XPath;
using System.Reflection;
using System.Runtime.Intrinsics.X86;
using Frontend.Models;
using Frontend.Resultados.Stock;
using Microsoft.AspNetCore.Mvc;
using Frontend.Comandos.Stock;
using Frontend.Comandos.Tratamientos;
using Microsoft.EntityFrameworkCore;

namespace Frontend.Controllers;

[ApiController]

public class StockController : ControllerBase
{

    private readonly FarmFoodNutricionContext _context;

    public StockController(FarmFoodNutricionContext context)
    {
        _context = context;
    }

    [HttpPost]
    [Route("api/stock/altaStock")]

    public async Task<ActionResult<ResultadoAltaStock>> AltaStock([FromBody] ComandoStock stock)
    {
        var alimento = await _context.Alimentos.FindAsync(stock.IdAlimento);
        var result = new ResultadoAltaStock();
        if (alimento == null)
        {
            result.SetError("Alimento no encontrado");
            result.StatusCode = "500";
            return Ok(result);
        }
        var stock1 = new StockAlimento
        {
            //IdStock = stock.IdStock,
            IdAlimento = stock.IdAlimento,
            FechaRegistro = DateOnly.ParseExact(stock.FechaRegistro, "yyyy-MM-dd", null),
            Toneladas = stock.Toneladas,
            PrecioTonelada = stock.PrecioTonelada,
            IdTipoMovimiento = 4

        };

        await _context.AddAsync(stock1);
        await _context.SaveChangesAsync();

        result.IdAlimento = stock.IdAlimento;
        result.FechaRegistro = stock.FechaRegistro;
        result.Toneladas = stock.Toneladas;
        //result.PrecioTonelada= (stock.PrecioTonelada);
        result.StatusCode = "200";
        return Ok(result);
    }

    [HttpPost("api/stock/getIngresoStockPorFechas")]
    public async Task<ActionResult<ResultadoListStockPorFecha>> GetStockPorFecha(ComandoFechas comandoFechas)
    {
        var result = new ResultadoListStockPorFecha();
        if (string.IsNullOrEmpty(comandoFechas.FechaInicio) || string.IsNullOrEmpty(comandoFechas.FechaFin))
        {
            result.SetError("Fechas incorrectas");
            result.StatusCode = "500";
            return Ok(result);
        }
        else
        {
            // Convertir las fechas del comando a DateOnly
            DateOnly fechaInicio = DateOnly.Parse(comandoFechas.FechaInicio);
            DateOnly fechaFin = DateOnly.Parse(comandoFechas.FechaFin);

            var stockPorFecha = await _context.StockAlimentos
                .Where(s => s.FechaRegistro >= fechaInicio && s.FechaRegistro <= fechaFin)
                .Include(s => s.IdAlimentoNavigation)
                .Include(s => s.IdTipoMovimientoNavigation)
                .OrderByDescending(s => s.FechaRegistro)
                .Select(s => new ResultadoListStockPorFechaItem
                {
                    IdStock = s.IdStock,
                    Alimento = s.IdAlimentoNavigation.NombreAlimento,
                    FechaRegistro = s.FechaRegistro,
                    Toneladas = s.Toneladas,
                    PrecioTonelada = s.PrecioTonelada,
                    TipoMovimiento = s.IdTipoMovimientoNavigation.NombreMovimiento,

                })
                .ToListAsync();

            var resultado = new ResultadoListStockPorFecha
            {
                listaStockPorFecha = stockPorFecha
            };

            return resultado;
        }
    }

    [HttpGet("api/stock/getStock")]
    public async Task<ActionResult<ResultadoListStock>> GetStock()
    {
        var result = new ResultadoListStock();
        var stock = await _context.StockAlimentos
            .Include(s => s.IdAlimentoNavigation)
            .Include(s => s.IdTipoMovimientoNavigation)
            .OrderByDescending(s => s.FechaRegistro)
            .Select(s => new ResultadoListStockItem
            {
                IdStock = s.IdStock,
                Alimento = s.IdAlimentoNavigation.NombreAlimento,
                FechaRegistro = s.FechaRegistro,
                Toneladas = s.Toneladas,
                PrecioTonelada = s.PrecioTonelada,
                TipoMovimiento = s.IdTipoMovimientoNavigation.NombreMovimiento,

            })
            .ToListAsync();

        var resultado = new ResultadoListStock
        {
            listaStock = stock
        };

        return result;

    }

    [HttpGet("api/stock/{idAlimento}")]
    public async Task<ActionResult<ResultadoStockPorIdDeAlimento>> ConsultarStockPorAli(int idAlimento)
    {
        // Obtener el alimento correspondiente al ID enviado en la solicitud
        var alimento = await _context.Alimentos.FirstOrDefaultAsync(a => a.IdAlimento == idAlimento);
        if (alimento == null)
        {
            return BadRequest($"No se encontró un alimento con el ID {idAlimento}");
        }
        // Consultar el stock del alimento en la tabla StockAlimento y calcular el stock acumulado
        var stockAcumulado = await _context.StockAlimentos
            .Where(s => s.IdAlimento == idAlimento)
            .Include(s => s.IdAlimentoNavigation)
            .SumAsync(s => s.Toneladas);

        // Consultar el stock del alimento en la tabla StockAlimento y transformar los resultados en objetos ResultadoListStockPorFechaItem
        var resultado = new ResultadoStockPorIdDeAlimento
        {
            IdAlimento = alimento.IdAlimento,
            NombreAlimento = alimento.NombreAlimento,
            Toneladas = stockAcumulado
        };

        // Actualizar el stock acumulado en cada objeto ResultadoListStockPorFechaItem
        return resultado;
    }


    [HttpPost]
    [Route("api/stock/registrarMovi")]

    public async Task<ActionResult<ResultadoAltaStock>> RegistrarMovi([FromBody] ComandoStock stock)
    {
        var alimento = await _context.Alimentos.FindAsync(stock.IdAlimento);
        var result = new ResultadoAltaStock();
        if (alimento == null)
        {
            result.SetError("Alimento no encontrado");
            result.StatusCode = "500";
            return Ok(result);
        }
        var stock1 = new StockAlimento
        {
            //IdStock = stock.IdStock,
            IdAlimento = stock.IdAlimento,
            FechaRegistro = DateOnly.ParseExact(stock.FechaRegistro, "yyyy-MM-dd", null),
            Toneladas = stock.Toneladas,
            PrecioTonelada = stock.PrecioTonelada,
            IdTipoMovimiento = stock.IdTipoMovimiento

        };

        await _context.AddAsync(stock1);
        await _context.SaveChangesAsync();

        result.IdAlimento = stock.IdAlimento;
        result.FechaRegistro = stock.FechaRegistro;
        result.Toneladas = stock.Toneladas;
        //result.PrecioTonelada= (stock.PrecioTonelada);
        result.StatusCode = "200";
        return Ok(result);
    }


}
