using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

[ApiController]
[Route("api/usuarios")]
public class ApiUsuariosController : ControllerBase
{
    // Método para haser las operaciones CRUD
    // C = Create
    // R = Reate
    // U = Update
    // D = Delete

    private readonly IMongoCollection<Usuarios> collection;
    public ApiUsuariosController()
    {
        var client = new MongoClient(CadenaConexion.MONGO_DB);
        var database = client.GetDatabase("Escuela_Sari_Jose");
        this.collection = database.GetCollection<Usuarios>("Usuarios");
    }

    [HttpGet]
        public IActionResult ListarUsuarios()
        {
            var filter = FilterDefinition<Usuarios>.Empty;
            var list = this.collection.Find(filter).ToList();
            return Ok(list);
        }
}
