namespace Frontend.Resultados.Especies;

public class ResultadoListEspecies: ResultadoBase
{
    public List<ResultadoListEspeciesItem> listaEspecies {get; set;} = new List<ResultadoListEspeciesItem>();
}

public class ResultadoListEspeciesItem{ 

    public int IdEspecie {get; set;}= 0;
    public string NombreEspecie {get; set;}= null!;

}