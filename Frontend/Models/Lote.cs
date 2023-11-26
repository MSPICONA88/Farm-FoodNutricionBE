using System;
using System.Collections.Generic;

namespace Frontend.Models;

public partial class Lote
{
    public int IdLote { get; set; }

    public DateOnly FechaIngreso { get; set; }

    public int CantidadAnimales { get; set; }

    public double PesoIngreso { get; set; }

    public int IdFinalidad { get; set; }

    public int IdRaza { get; set; }

    public int EdadMeses { get; set; }

    public DateOnly? FechaEgreso { get; set; }

    public double? PesoEgreso { get; set; }

    public int CantidadActual { get; set; }

    public virtual ICollection<Animale> Animales { get; } = new List<Animale>();

    public virtual Finalidade IdFinalidadNavigation { get; set; } = null!;

    public virtual Raza IdRazaNavigation { get; set; } = null!;

    public virtual ICollection<PlanesAlimentacion> PlanesAlimentacions { get; } = new List<PlanesAlimentacion>();
}
