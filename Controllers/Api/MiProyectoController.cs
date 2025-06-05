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
    public IActionResult Presentacion(){
        MongoClient client=new
        MongoClient(CadenaConexion.MONGO_DB);
        var db=
        client.GetDatabase("Escuela_Sari_Jose");
        var collection=
        db.GetCollection<Equipo>("Equipo");

        var list=collection.Find(FilterDefinition<Equipo>.Empty).ToList();
        return Ok(list);
    }
}