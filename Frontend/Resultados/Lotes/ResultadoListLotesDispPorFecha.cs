namespace Frontend.Resultados.Lotes;

public class ResultadoListLotesDispPorFecha: ResultadoBase
{
    public List<ResultadoListLotesDispPorFechaItem> listaLotesDispPorFecha {get; set;} = new List<ResultadoListLotesDispPorFechaItem>();
}

public class ResultadoListLotesDispPorFechaItem
{
    public int IdLote { get; set; }
    
    public DateOnly FechaIngreso { get; set; }

    public int CantidadActual { get; set; }

    public int CantidadBajas { get; set; }

    public double PesoIngreso { get; set; }

    public string Finalidad { get; set; }= null!;

    public string Especie { get; set; }= null!;

    public string Raza { get; set; }= null!;

    public int EdadMeses { get; set; }
}
