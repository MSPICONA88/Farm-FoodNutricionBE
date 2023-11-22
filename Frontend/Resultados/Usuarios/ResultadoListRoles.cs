namespace Frontend.Resultados.Usuarios;

public class ResultadoListRoles: ResultadoBase
{
    
    public List<ResultadoListRolesItem> listaRoles { get; set; } = new List<ResultadoListRolesItem>();
    

    public class ResultadoListRolesItem
    {
        public int IdRol { get; set; } = 0;
        public string NombreRol { get; set; } = null!;
        
    }
}
