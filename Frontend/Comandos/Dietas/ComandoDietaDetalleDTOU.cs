namespace Frontend.Comandos.Dietas;

public class ComandoDietaDetalleDTOU
{
    public int IdDieta { get; set; }
    public string NombreDieta { get; set; }
    public String FechaCreacion { get; set; }
    public string Observacion { get; set; }
    public List<AlimentoDetalleDTO2> Alimentos { get; set; }
}

public class AlimentoDetalleDTO2
{
    public int IdAlimento { get; set; }
    //public string NombreAlimento { get; set; }
    public double Porcentaje { get; set; }
}

