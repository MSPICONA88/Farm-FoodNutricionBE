using System;
using System.Collections.Generic;

namespace Frontend.Models;

public partial class Alimentacione
{
    public int IdAlimentacion { get; set; }

    public int IdPlan { get; set; }

    public DateOnly FechaAlimentacion { get; set; }

    public double ToneladasDispensadas { get; set; }

    public virtual PlanesAlimentacion IdPlanNavigation { get; set; } = null!;
}
