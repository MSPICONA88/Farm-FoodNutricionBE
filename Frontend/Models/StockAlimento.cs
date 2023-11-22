using System;
using System.Collections.Generic;

namespace Frontend.Models;

public partial class StockAlimento
{
    public int IdStock { get; set; }

    public int IdAlimento { get; set; }

    public DateOnly FechaRegistro { get; set; }

    public double Toneladas { get; set; }

    public decimal PrecioTonelada { get; set; }

    public int IdTipoMovimiento { get; set; }

    public virtual Alimento IdAlimentoNavigation { get; set; } = null!;

    public virtual TiposMovimiento IdTipoMovimientoNavigation { get; set; } = null!;
}
