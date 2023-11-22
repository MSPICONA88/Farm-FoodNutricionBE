using System;
using System.Collections.Generic;

namespace Frontend.Models;

public partial class Raza
{
    public int IdRaza { get; set; }

    public string NombreRaza { get; set; } = null!;

    public int IdEspecie { get; set; }

    public virtual Especy IdEspecieNavigation { get; set; } = null!;

    public virtual ICollection<Lote> Lotes { get; } = new List<Lote>();
}
