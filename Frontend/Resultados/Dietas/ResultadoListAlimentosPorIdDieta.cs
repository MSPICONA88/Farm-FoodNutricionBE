namespace Frontend.Resultados.Dietas;

public class ResultadoListAlimentosPorIdDieta: ResultadoBase
{
    public List<ResultadoListAlimentosPorIdDietaItem> listaAlimentos { get; set; } = new List<ResultadoListAlimentosPorIdDietaItem>();

    public class ResultadoListAlimentosPorIdDietaItem
    {
        public int IdDieta { get; set; }
        public string NombreDieta { get; set; }=null!;
        public string NombreAlimento { get; set; }=null!;
        public double Porcentaje { get; set; }
    }
}
