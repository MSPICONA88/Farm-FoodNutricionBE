namespace Frontend.Models;

public class AnimalExistence
{
    public string Especie { get; set; }
    public string Raza { get; set; }
    public int CantidadAnimales { get; set; }
    public int IdEspecie { get; internal set; }
    public int IdRaza { get; internal set; }
}
