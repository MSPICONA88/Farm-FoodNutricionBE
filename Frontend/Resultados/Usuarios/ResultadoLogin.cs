namespace Frontend.Resultados;

public class ResultadoLogin: ResultadoBase
{
    public string IdUsuario {get; set;}
    public string NombreUsuario {get; set;}= null!;
    
    public string Rol {get; set;}= null!;

    
}
