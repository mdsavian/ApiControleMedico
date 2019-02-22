using ApiControleMedico.Modelos;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace ApiControleMedico.Repositorio
{
    public class DbContexto<T>
    {
        public IMongoDatabase Database { get; }
        public IMongoCollection<T> Collection { get; }


        public DbContexto(string collectionName)
        {
            var client = new MongoClient("mongodb://localhost:27017");

            Database = client.GetDatabase("ControleMedicoDb");
            Collection = Database.GetCollection<T>(collectionName);

            RegisterMapIfNeeded<Paciente>();
            RegisterMapIfNeeded<Pessoa>();
            RegisterMapIfNeeded<Usuario>();
            RegisterMapIfNeeded<Medico>();
            RegisterMapIfNeeded<Convenio>();
            RegisterMapIfNeeded<ConvenioMedico>();
        }

        // Check to see if map is registered before registering class map
        // This is for the sake of the polymorphic types that we are using so Mongo knows how to deserialize
        public void RegisterMapIfNeeded<TClass>()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(TClass)))
                BsonClassMap.RegisterClassMap<TClass>();
        }
    }
}
