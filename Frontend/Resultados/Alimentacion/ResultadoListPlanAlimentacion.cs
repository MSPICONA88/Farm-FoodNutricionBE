namespace Frontend.Resultados.Alimentacion;

public class ResultadoListPlanAlimentacion: ResultadoBase
{
   
      public List<ResultadoListPlanAlimentacionItem> listaPlanesAlimentacion {get; set;} = new List<ResultadoListPlanAlimentacionItem>();
}
public class ResultadoListPlanAlimentacionItem
{
    public int IdPlan { get; set; }
    public int IdLote { get; set; }
    public string NombreEspecie { get; set; }=null!;
    public string Raza { get; set; }=null!;
    public int Cantidad { get; set; }
    public string NombreDieta { get; set; }=null!;
    public DateOnly FechaInicio { get; set; }
    public DateOnly FechaFin { get; set; }
    public double CantPorDiaPorAnimal { get; set; }
    public double CantToneladaDiaria { get; set; }
}
