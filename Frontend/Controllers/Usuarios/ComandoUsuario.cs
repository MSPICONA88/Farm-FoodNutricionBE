using Frontend.Models;

namespace Frontend.Comandos.Usuarios;

public class ComandoUsuario
{
    
    //public int IdUsuario { get; set; }

    public string NombreApellido { get; set; } = null!;

    public string Usuario1 { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int IdRol { get; set; }

    //public DateOnly FechaCreacion { get; set; }

    //public virtual Role IdRolNavigation { get; set; } = null!;
}
