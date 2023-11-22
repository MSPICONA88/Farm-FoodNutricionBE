using System;
using System.Collections.Generic;

namespace Frontend.Models;

public partial class TratamientoLote
{
    public int IdTratLote { get; set; }

    public int IdLote { get; set; }

    public int IdTipoTrat { get; set; }

    public string Medicacion { get; set; } = null!;

    public DateOnly FechaInicio { get; set; }

    public DateOnly FechaFin { get; set; }

    public virtual Lote IdLoteNavigation { get; set; } = null!;

    public virtual TiposTratamiento IdTipoTratNavigation { get; set; } = null!;
}
