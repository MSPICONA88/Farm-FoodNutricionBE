using System;
using System.Collections.Generic;

namespace Frontend.Models;

public partial class Dieta
{
    public int IdDieta { get; set; }

    public string NombreDieta { get; set; } = null!;

    public DateOnly FechaCreacion { get; set; }

    public string? Observacion { get; set; }

    public virtual ICollection<AlimentosxDietum> AlimentosxDieta { get; } = new List<AlimentosxDietum>();

    public virtual ICollection<PlanesAlimentacion> PlanesAlimentacions { get; } = new List<PlanesAlimentacion>();
}
