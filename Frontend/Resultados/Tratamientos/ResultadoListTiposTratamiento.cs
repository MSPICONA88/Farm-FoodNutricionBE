namespace Frontend.Resultados.Tratamientos;

public class ResultadoListTiposTratamiento: ResultadoBase
{
    public List<ResultadoListTiposTratamientoItem> listaTiposTrat {get; set;} = new List<ResultadoListTiposTratamientoItem>();
}

public class ResultadoListTiposTratamientoItem{ 
    
    public int IdTipoTratamiento {get; set;}= 0;
    public string Descripcion {get; set;}= null!;

}