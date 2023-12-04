namespace Frontend.Resultados.Alimentacion;

public class ResultadoPlanPorId: ResultadoBase
{
    public int IdPlan { get; set; }

    public int IdLote { get; set; }

    public int IdDieta { get; set; }

    public DateOnly FechaInicio { get; set; }

    public DateOnly FechaFin { get; set;}

    public double CantToneladaDiaria { get; set; }
}
