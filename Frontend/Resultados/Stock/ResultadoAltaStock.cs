namespace Frontend.Resultados.Stock;

public class ResultadoAltaStock: ResultadoBase
{
    public int IdAlimento { get; set; }
    public String FechaRegistro { get; set; }=null!;
    public double Toneladas { get; set; }
    //public double PrecioTonelada { get; set; }

}
