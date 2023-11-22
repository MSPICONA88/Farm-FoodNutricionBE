using System.Collections.Generic;
namespace Frontend.Resultados.Usuarios;

public class ResultadoListUsuarios: ResultadoBase
{
    public List<ResultadoListUsuariosItem> listaUsuarios {get; set;} = new List<ResultadoListUsuariosItem>();
}

public class ResultadoListUsuariosItem{ 
    public string NombreApellido {get; set;}= null!;
    public string NombreUsuario {get; set;}= null!;
    //public string Password {get; set;}= null!;
    public string Email {get; set;}= null!;
    public string Rol {get; set;}= null!;

    
}