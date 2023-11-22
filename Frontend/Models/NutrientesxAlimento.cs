using System;
using System.Collections.Generic;

namespace Frontend.Models;

public partial class NutrientesxAlimento
{
    public int IdNutrientexalimento { get; set; }

    public int IdAlimento { get; set; }

    public int IdNutriente { get; set; }

    public int Porcentaje { get; set; }

    public virtual Alimento IdAlimentoNavigation { get; set; } = null!;

    public virtual Nutriente IdNutrienteNavigation { get; set; } = null!;
}
