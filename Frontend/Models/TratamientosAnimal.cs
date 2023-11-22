using System;
using System.Collections.Generic;

namespace Frontend.Models;

public partial class TratamientosAnimal
{
    public int IdTratAnimal { get; set; }

    public int IdAnimal { get; set; }

    public int IdTipoTrat { get; set; }

    public string Medicacion { get; set; } = null!;

    public DateOnly FechaInicio { get; set; }

    public DateOnly FechaFin { get; set; }

    public virtual Animale IdAnimalNavigation { get; set; } = null!;

    public virtual TiposTratamiento IdTipoTratNavigation { get; set; } = null!;
}
