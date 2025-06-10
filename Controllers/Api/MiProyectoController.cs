using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

[ApiController]
[Route("mi-proyecto")]
public class MiProyectoController : ControllerBase
{
    [HttpGet("Integrantes")]
    public ActionResult<MiProyecto>Integrantes()
    {
        var proyecto = new MiProyecto
        {
            Integrante1="Sari Ameli Soni Lopez",
            Integrante2="José Manuel Mar Escobar"
        };

        return Ok(proyecto);
    }

    [HttpGet("presentacion")]
    public IActionResult Presentacion()
    {
        
        var client = new MongoClient(CadenaConexion.MONGO_DB);
        var database= client.GetDatabase("Escuela_Sari_Jose");
        var collection=database.GetCollection<Equipo>("Equipo");

        var filter = FilterDefinition<Equipo>.Empty;
        var item = collection.Find(filter).FirstOrDefault();
        return Ok(item);
    }
}