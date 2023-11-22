using System;
using System.Collections.Generic;

namespace Frontend.Models;

public partial class Finalidade
{
    public int IdFinalidad { get; set; }

    public string NombreFinalidad { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual ICollection<Lote> Lotes { get; } = new List<Lote>();
}
