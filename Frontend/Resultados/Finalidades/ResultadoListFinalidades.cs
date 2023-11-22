namespace Frontend.Resultados.Finalidades;

public class ResultadoListFinalidades: ResultadoBase
{
    public List<ResultadoListFinalidadesItem> listaFinalidades {get; set;} = new List<ResultadoListFinalidadesItem>();
}

public class ResultadoListFinalidadesItem{ 
    
    public int IdFinalidad {get; set;}= 0;
    public string NombreFinalidad {get; set;}= null!;

}