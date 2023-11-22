using Frontend.Comandos.Dietas;

namespace Frontend.Resultados.Dietas;

public class ResultadoListDietasConAlimentos : ResultadoBase
{
    public List<ResultadoListDietasConAlimentosItem> listaDietas { get; set; } = new List<ResultadoListDietasConAlimentosItem>();
}

public class ResultadoListDietasConAlimentosItem
{
    public int IdDieta { get; set; }
    public string NombreDieta { get; set; }
    public string FechaCreacion { get; set; }
    public string? Observacion { get; set; }
    public List<AlimentoDetalleDTO> Alimentos { get; set; }
}