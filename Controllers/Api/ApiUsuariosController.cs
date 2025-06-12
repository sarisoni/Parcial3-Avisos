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
    public IActionResult ListarUsuarios(string? texto)
    {
        var filter = FilterDefinition<Usuarios>.Empty;
        if (!string.IsNullOrWhiteSpace(texto))
        {
            var filterNombre = Builders<Usuarios>.Filter.Regex(u => u.Nombre, new BsonRegularExpression(texto, "i"));
            var filterCorreo = Builders<Usuarios>.Filter.Regex(u => u.Correo, new BsonRegularExpression(texto, "i"));
            filter = Builders<Usuarios>.Filter.Or(filterNombre, filterCorreo);

        }
        var list = this.collection.Find(filter).ToList();

        return Ok(list);
    }
    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        var filter = Builders<Usuarios>.Filter.Eq(x => x.Id, id);
        var item = this.collection.Find(filter).FirstOrDefault();
        if (item != null)
        {
            this.collection.DeleteOne(filter);
        }
        return NoContent();
    }
}
