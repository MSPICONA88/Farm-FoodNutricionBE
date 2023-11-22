namespace Frontend.Resultados.Alimentacion;

public class ResultadoAltaPlanAlimentacion : ResultadoBase
{
    public int IdAlimentacion { get; set; }
    public int IdLote { get; set; }
    public int IdDieta { get; set; }
    public DateOnly FechaInicio { get; set; }
    public DateOnly FechaFin { get; set; }
    public double ToneladasDispensadas { get; set; }
}