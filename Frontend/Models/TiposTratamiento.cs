using System;
using System.Collections.Generic;

namespace Frontend.Models;

public partial class TiposTratamiento
{
    public int IdTipoTrat { get; set; }

    public string Decripcion { get; set; } = null!;

    public virtual ICollection<TratamientosAnimal> TratamientosAnimals { get; } = new List<TratamientosAnimal>();
}
