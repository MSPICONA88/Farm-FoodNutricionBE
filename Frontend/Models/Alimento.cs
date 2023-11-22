using System;
using System.Collections.Generic;

namespace Frontend.Models;

public partial class Alimento
{
    public int IdAlimento { get; set; }

    public string NombreAlimento { get; set; } = null!;

    public virtual ICollection<AlimentosxDietum> AlimentosxDieta { get; } = new List<AlimentosxDietum>();

    public virtual ICollection<NutrientesxAlimento> NutrientesxAlimentos { get; } = new List<NutrientesxAlimento>();

    public virtual ICollection<StockAlimento> StockAlimentos { get; } = new List<StockAlimento>();
}
