namespace Frontend.Comandos.Stock;

public class ComandoStock
{
    public int IdAlimento { get; set; }

    public String FechaRegistro { get; set; }=null!;

    public double Toneladas { get; set; }

    public decimal PrecioTonelada { get; set; }

    public int IdTipoMovimiento { get; set; }
}
