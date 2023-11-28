namespace Frontend.Resultados.Stock;

public class ResultadoListStock: ResultadoBase
{
    public List<ResultadoListStockItem> listaStock {get; set;} = new List<ResultadoListStockItem>();
}


public class ResultadoListStockItem{ 
    public int IdStock { get; set; }
    public String Alimento { get; set; }= null!;
    public DateOnly FechaRegistro { get; set; }
    public double Toneladas { get; set; }
    public decimal PrecioTonelada { get; set; }
    public string TipoMovimiento { get; set; }= null!;
}
