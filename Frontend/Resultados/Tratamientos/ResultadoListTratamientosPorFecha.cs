namespace Frontend.Resultados.Tratamientos;

public class ResultadoListTratamientosPorFecha: ResultadoBase
{
     public List<ResultadoListTratamientosPorFechaItem> listaTratatamientosPorFecha {get; set;} = new List<ResultadoListTratamientosPorFechaItem>();
}

public class ResultadoListTratamientosPorFechaItem{ 
    
    public string Especie{get; set;}= null!;
    public string Raza{get; set;}= null!;
    public string NombreTratamiento{get; set;}= null!;
    public string Medicacion {get; set;}= null!;
    public DateOnly FechaInicio {get; set;}
    public DateOnly FechaFin {get; set;}
    public int DiasDeTratamiento{get; set;}= 0;
}
