using System;
using System.Collections.Generic;

namespace Frontend.Models;

public partial class Especy
{
    public int IdEspecie { get; set; }

    public string NombreEspecie { get; set; } = null!;

    public virtual ICollection<Raza> Razas { get; } = new List<Raza>();
}
