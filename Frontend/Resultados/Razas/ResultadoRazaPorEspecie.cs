namespace Frontend.Resultados.Razas;

public class ResultadoRazaPorEspecie: ResultadoBase
{
    public List<ResultadoListRazasPorEspecieItem> listaRazasPorEspecie {get; set;} = new List<ResultadoListRazasPorEspecieItem>();
}

public class ResultadoListRazasPorEspecieItem{ 

    public int IdRaza {get; set;}= 0;

    public string NombreRaza {get; set;}= null!;

}
