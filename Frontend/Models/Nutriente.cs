using System;
using System.Collections.Generic;

namespace Frontend.Models;

public partial class Nutriente
{
    public int IdNutriente { get; set; }

    public string NombreNutriente { get; set; } = null!;

    public virtual ICollection<NutrientesxAlimento> NutrientesxAlimentos { get; } = new List<NutrientesxAlimento>();
}
