namespace Frontend.Resultados.Tratamientos;

public class ResultadoAltaTratamientoPorAnimal:ResultadoBase
{
    public int IdAnimal { get; set; }

    public int IdTipoTrat { get; set; }

    public string Medicacion { get; set; } = null!;

    public DateOnly FechaInicio { get; set; }

    public DateOnly FechaFin { get; set; }
}
