namespace Frontend.Resultados.Alimentos;

public class ResultadoAliPorDieta: ResultadoBase
{
    public int IdAlimento { get; set; }
    public int IdDieta { get; set; }
    public double Porcentaje { get; set; }
}
