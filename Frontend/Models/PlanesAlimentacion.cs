using System;
using System.Collections.Generic;

namespace Frontend.Models;

public partial class PlanesAlimentacion
{
    public int IdPlan { get; set; }

    public int IdLote { get; set; }

    public int IdDieta { get; set; }

    public DateOnly FechaInicio { get; set; }

    public DateOnly FechaFin { get; set; }

    public double CantToneladaDiaria { get; set; }

    public virtual ICollection<Alimentacione> Alimentaciones { get; } = new List<Alimentacione>();

    public virtual Dieta IdDietaNavigation { get; set; } = null!;

    public virtual Lote IdLoteNavigation { get; set; } = null!;
}
