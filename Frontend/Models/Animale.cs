using System;
using System.Collections.Generic;

namespace Frontend.Models;

public partial class Animale
{
    public int IdAnimal { get; set; }

    public int IdLote { get; set; }

    public virtual Lote IdLoteNavigation { get; set; } = null!;

    public virtual ICollection<TratamientosAnimal> TratamientosAnimals { get; } = new List<TratamientosAnimal>();
}
