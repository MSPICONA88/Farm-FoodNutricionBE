using Frontend.Resultados;

public class ResultadoListMovimientos: ResultadoBase
{
    public List<ResultadoListMovimientosItem> listaMovimientos {get; set;} = new List<ResultadoListMovimientosItem>();
}

public class ResultadoListMovimientosItem{ 
    
    public int IdMovimiento {get; set;}= 0;
    public string NombreMovimiento {get; set;}= null!;
    public string Descripcion {get; set;}= null!;

}