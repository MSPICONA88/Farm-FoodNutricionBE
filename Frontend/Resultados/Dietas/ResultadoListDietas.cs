namespace Frontend.Resultados.Dietas;

public class ResultadoListDietas: ResultadoBase
{

public List<ResultadoListDietasItem> listaDietas {get; set;} = new List<ResultadoListDietasItem>();
}

public class ResultadoListDietasItem{ 
    
    public int IdDieta {get; set;}= 0;
    public String NombreDieta { get; set; } = null!;
    public String FechaCreacion { get; set; }
    public String? Observacion { get; set; }
    

}

