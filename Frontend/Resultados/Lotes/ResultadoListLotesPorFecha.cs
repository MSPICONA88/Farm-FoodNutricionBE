namespace Frontend.Resultados.Lotes;

public class ResultadoListLotesPorFecha: ResultadoBase
{
    public List<ResultadoListLotesPorFechaItem> listaLotesPorFecha {get; set;} = new List<ResultadoListLotesPorFechaItem>();
}


public class ResultadoListLotesPorFechaItem{ 
    public int IdLote { get; set; }
    
    public DateOnly FechaIngreso { get; set; }

    public int CantidadAnimales { get; set; }

    public double PesoTotal { get; set; }

    public string Finalidad { get; set; }= null!;

    public string Especie { get; set; }= null!;

    public string Raza { get; set; }= null!;

    public int EdadMeses { get; set; }
}
