namespace Frontend.Resultados.Animales;

public class ResultadoListAnimales: ResultadoBase
{

public List<ResultadoListAnimalesItem> listaAnimales {get; set;} = new List<ResultadoListAnimalesItem>();
}

public class ResultadoListAnimalesItem{ 
    
    public int IdAnimal {get; set;}= 0;
    

}


