namespace Frontend.Comandos.Lotes;

public class ComandoUpdateLote
{
    public String FechaIngreso { get; set; }

    public int CantidadAnimales { get; set; }

    public double PesoIngreso { get; set; }

    public int IdFinalidad { get; set; }

    public int IdRaza { get; set; }

    public int EdadMeses { get; set; }

    public String? FechaEgreso { get; set; }

    public double? PesoEgreso { get; set; }=0;

    // public int CantidadFinal { get; set; }
}
