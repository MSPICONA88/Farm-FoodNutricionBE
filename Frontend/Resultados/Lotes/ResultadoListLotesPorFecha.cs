namespace Frontend.Resultados.Lotes;

public class ResultadoListLotesPorFecha: ResultadoBase
{
    public List<ResultadoListLotesPorFechaItem> listaLotesPorFecha {get; set;} = new List<ResultadoListLotesPorFechaItem>();
}


public class ResultadoListLotesPorFechaItem{ 
    public int IdLote { get; set; }
    
    public DateOnly FechaIngreso { get; set; }

    public int CantidadAnimales { get; set; }

    public double PesoIngreso { get; set; }

    public string Finalidad { get; set; }= null!;

    public string Especie { get; set; }= null!;

    public string Raza { get; set; }= null!;

    public int EdadMeses { get; set; }

    public int? CantidadActual { get; set; }
    public int? CantidadBajas { get; set; }
    public DateOnly? FechaEgreso { get; set; }
    public double? PesoEgreso { get; set; }
    
}
