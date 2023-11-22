namespace Frontend.Resultados.Stock;

public class ResultadoListStockPorFecha: ResultadoBase
{
    public List<ResultadoListStockPorFechaItem> listaStockPorFecha {get; set;} = new List<ResultadoListStockPorFechaItem>();
}


public class ResultadoListStockPorFechaItem{ 
    public int IdStock { get; set; }
    public String Alimento { get; set; }= null!;
    public DateOnly FechaRegistro { get; set; }
    public double Toneladas { get; set; }
    public decimal PrecioTonelada { get; set; }
    public string TipoMovimiento { get; set; }= null!;
}
