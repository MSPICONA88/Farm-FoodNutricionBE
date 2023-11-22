using System.Collections.Generic;
namespace Frontend.Resultados.Alimentos;

public class ResultadoListAlimentos: ResultadoBase
{
    public List<ResultadoListAlimentosItem> listaAlimentos {get; set;} = new List<ResultadoListAlimentosItem>();
}

public class ResultadoListAlimentosItem
{ 
    public int IdAlimento {get; set;}
    public string NombreAlimento {get; set;}= null!;

}