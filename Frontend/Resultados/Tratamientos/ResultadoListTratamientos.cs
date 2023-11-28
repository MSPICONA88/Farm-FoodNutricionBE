using Frontend.Resultados;
namespace Frontend.Resultados.Tratamientos;


public class ResultadoListTratamientos: ResultadoBase
{
     public List<ResultadoListTratamientosItem> listaTratatamientos {get; set;} = new List<ResultadoListTratamientosItem>();
}

public class ResultadoListTratamientosItem{ 
    
    public string Especie{get; set;}= null!;
    public string Raza{get; set;}= null!;
    public string NombreTratamiento{get; set;}= null!;
    public string Medicacion {get; set;}= null!;
    public DateOnly FechaInicio {get; set;}
    public DateOnly FechaFin {get; set;}
    public int DiasDeTratamiento{get; set;}= 0;
}