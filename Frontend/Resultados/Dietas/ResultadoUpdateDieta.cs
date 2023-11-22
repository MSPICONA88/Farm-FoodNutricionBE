namespace Frontend.Resultados.Dietas;

public class ResultadoUpdateDieta: ResultadoBase

{
    public string NombreDieta { get; set; }
    public string FechaCreacion { get; set; }
    public string Observacion { get; set; }
    public List<AlimentoDietaDTO2> Alimentos { get; set; }
}

public class AlimentoDietaDTO2
{
    public int IdAlimento { get; set; }
    public double Porcentaje { get; set; }
}
