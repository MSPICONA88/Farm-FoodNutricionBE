namespace Frontend.Resultados.Alimentacion;

public class ResultadoRegistrarAlimentacion: ResultadoBase
{
    public int IdPlan { get; set; }
    public String FechaAlimentacion { get; set; }=null!;
    public int ToneladasDispensadas { get; set; }
}
