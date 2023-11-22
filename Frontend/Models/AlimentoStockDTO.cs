namespace Frontend.Models;

public class AlimentoStockDTO
{
    public int IdAlimento { get; set; }
    public string NombreAlimento { get; set; }
    public decimal StockActual { get; set; }
    public decimal StockNecesario { get; set; }
    public decimal CantidadAComprar { get; set; }
    public string Estado { get; set; }
}
