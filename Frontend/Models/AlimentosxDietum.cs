using System;
using System.Collections.Generic;

namespace Frontend.Models;

public partial class AlimentosxDietum
{
    public int IdAlimentoDieta { get; set; }

    public int IdAlimento { get; set; }

    public int IdDieta { get; set; }

    public int Porcentaje { get; set; }

    public virtual Alimento IdAlimentoNavigation { get; set; } = null!;

    public virtual Dieta IdDietaNavigation { get; set; } = null!;
}
