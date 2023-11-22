namespace Frontend.Resultados.Lotes;

public class ResultadoLotePorId: ResultadoBase
{
    public int IdLote { get; set; }
    public DateOnly FechaIngreso { get; set; }

    public int CantidadAnimales { get; set; }

    public double PesoTotal { get; set; }

    public int IdFinalidad { get; set; }

    public int IdEspecie { get; set; }

    public int IdRaza { get; set; }

    public int EdadMeses { get; set; }
}
