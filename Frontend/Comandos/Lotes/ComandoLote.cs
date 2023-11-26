namespace Frontend.Comandos.Usuarios;

public class ComandoLote
{
        //public int IdLote { get; set; }

    public DateOnly FechaIngreso { get; set; }

    public int CantidadAnimales { get; set; }

    public double PesoIngreso { get; set; }

    public int IdFinalidad { get; set; }

    public int IdRaza { get; set; }

    public int EdadMeses { get; set; }

    //public virtual Finalidade IdFinalidadNavigation { get; set; } = null!;

    //public virtual Raza IdRazaNavigation { get; set; } = null!;
}
