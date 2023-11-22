namespace Frontend.Resultados;

public class ResultadoBase
{
    public bool Ok {get; set;}=true;
    public string Error {get; set;}= null!;
    public string StatusCode {get; set;}= null!;

    public void SetError(string mensajeError){
        Ok =false;
        Error= mensajeError;
    }

}
