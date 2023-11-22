namespace Frontend.Models;

public class DietaDTO
{
    public string NombreDieta { get; set; }
    public string FechaCreacion { get; set; }
    public string Observacion { get; set; }
    public List<AlimentoDietaDTO> Alimentos { get; set; }
}

public class AlimentoDietaDTO
{
    public int IdAlimento { get; set; }
    public double Porcentaje { get; set; }
}