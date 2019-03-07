using System.Security.Authentication;
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

            //Conexão com produção
            //string connectionString =
            //    @"mongodb://controlemedico:DJdM5g8hFGmg1CtqWVwSw2Vw7kkv83aVGK0LTwagzpagUftQCorpWh8URLpd1EFNASBj2gPBhSJ0oFCzioBkfg==@controlemedico.documents.azure.com:10255/?ssl=true&replicaSet=globaldb";
            //MongoClientSettings settings = MongoClientSettings.FromUrl(
            //    new MongoUrl(connectionString)
            //);

            //settings.SslSettings =
            //    new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            //var client = new MongoClient(settings);

            //conexão debug
            var client = new MongoClient("mongodb://localhost:27017");

            Database = client.GetDatabase("ControleMedicoDb");
            Collection = Database.GetCollection<T>(collectionName);

            RegisterMapIfNeeded<Paciente>();
            RegisterMapIfNeeded<Pessoa>();
            RegisterMapIfNeeded<Usuario>();
            RegisterMapIfNeeded<Medico>();
            RegisterMapIfNeeded<Convenio>();
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
