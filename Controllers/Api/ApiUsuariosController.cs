using Microsoft.AspNetCore.Http.HttpResults;
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

    [HttpPut]
    public IActionResult Update(UsuarioRequest model)
    {

        // 1. Validar el modelo para que contenga datos
        if (string.IsNullOrWhiteSpace(model.Correo))
        {
            return BadRequest("El correo es requerido");
        }
        if (string.IsNullOrWhiteSpace(model.Password))
        {
            return BadRequest("El Password requerido");
        }

        if (string.IsNullOrWhiteSpace(model.Nombre))
        {
            return BadRequest("El nombre es requerido");
        }

        // Validar que el correo no exista 
        var filter = Builders<Usuarios>.Filter.Eq(x => x.Correo, model.Correo);
        var item = this.collection.Find(filter).FirstOrDefault();
        if (item != null)
        {
            return BadRequest("El correo " + model.Correo + " ya existe en la base de datos");
        }

        Usuarios bd = new Usuarios();
        bd.Nombre = model.Nombre;
        bd.Correo = model.Correo;
        bd.Password = model.Password;

        this.collection.InsertOne(bd);

        return Ok();
    }
    public IActionResult Update(string id, UsuarioRequest model)
    {
        // Validar que el correo no exista
        var filterCorreo = Builders<Usuarios>.Filter.Eq(x => x.Correo, model.Correo);
        var itemExistente = this.collection.Find(filterCorreo).FirstOrDefault();

        if (itemExistente != null && itemExistente.Id != id)
        {
            return BadRequest("El correo " + model.Correo + " ya existe en la base de datos");
        }

        var filter = Builders<Usuarios>.Filter.Eq(x => x.Id, id);

        var updateOptions = Builders<Usuarios>.Update
            .Set(x => x.Correo, model.Correo)
            .Set(x => x.Nombre, model.Nombre)
            .Set(x => x.Password, model.Password);

        this.collection.UpdateOne(filter, updateOptions);

        return Ok();
    }
}