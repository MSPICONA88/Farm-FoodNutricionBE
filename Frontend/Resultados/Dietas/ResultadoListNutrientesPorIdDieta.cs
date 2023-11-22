using System.Numerics;
using Frontend.Resultados;
using Frontend.Resultados.Alimentos;
namespace Frontend.Resultados.Dietas;

public class ResultadoListNutrientesPorIdDieta: ResultadoBase
{
    
    public List<ResultadoListNutrientesPorIdDietaItem> listaNutrientes {get; set;} = new List<ResultadoListNutrientesPorIdDietaItem>();
    
    public class ResultadoListNutrientesPorIdDietaItem{ 
    public string NombreNutriente { get; set; }=null!;
    public int Porcentaje { get; set; }
    }
}
