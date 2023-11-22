namespace Frontend.Resultados.Dietas;

public class ResultadoAltaDieta: ResultadoBase 
{
    public int IdDieta { get; set; }

    public string NombreDieta { get; set; } = null!;

    public DateOnly FechaCreacion { get; set; }

    public string? Observacion { get; set; }

    
}
