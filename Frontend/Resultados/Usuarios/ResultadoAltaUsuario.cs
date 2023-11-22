using Frontend.Models;

namespace Frontend.Resultados.Usuarios;

public class ResultadoAltaUsuario: ResultadoBase
{

    

    public string NombreApellido { get; set; } = null!;

    public string Usuario1 { get; set; } = null!;

    //public string Password { get; set; } = null!;
    
    public string Email { get; set; } = null!;

    public string Rol { get; set; } = null!;


    //public DateOnly FechaCreacion { get; set; }

    

    //public virtual Role IdRolNavigation { get; set; } = null!;
}
