namespace Frontend.Resultados.Lotes;

public class ResultadoListLotesPorEspecie
{
    public List<ResultadoListLotesPorEspecieItem> ListaLotesPorEspecie {get; set;} = new List<ResultadoListLotesPorEspecieItem>();
}


public class ResultadoListLotesPorEspecieItem{ 
    public int IdLote { get; set; }
    
    public DateOnly FechaIngreso { get; set; }

    public int CantidadAnimales { get; set; }

    public double PesoIngreso { get; set; }

    public string Finalidad { get; set; }= null!;

    public string Especie { get; set; }= null!;

    public string Raza { get; set; }= null!;

    public int EdadMeses { get; set; }

    public double PesoPromedioPorAnimalIngreso { get; set; }
    public double PesoAproxActual { get; set; }
    public int CantidadActual { get; set; }
}
