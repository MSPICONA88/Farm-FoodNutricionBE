using System;
using System.Collections.Generic;

namespace Frontend.Models;

public partial class TiposMovimiento
{
    public int IdMovimiento { get; set; }

    public string NombreMovimiento { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<StockAlimento> StockAlimentos { get; } = new List<StockAlimento>();
}
