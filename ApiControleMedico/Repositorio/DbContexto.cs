using System;
using System.Security.Authentication;
using ApiControleMedico.Modelos;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace ApiControleMedico.Repositorio
{
    public class DbContexto<T> : IDisposable
    {
        public IMongoDatabase Database { get; set; }
        public IMongoCollection<T> Collection { get; set; }
        public IClientSessionHandle Session { get; set; }

        public DbContexto(string collectionName, bool abreSecao = false)
        {
            var producao = false;

             
            string connectionString = producao ? @"mongodb://controlemedico02:controlemedico@mongodb.controlemedico.kinghost.net:27017/controlemedico02" : "mongodb://localhost:27017";
            var nomeBase = producao ? "controlemedico02" : "ControleMedicoDb";

            MongoClientSettings settings = MongoClientSettings.FromUrl(
                new MongoUrl(connectionString)
            );

            var client = new MongoClient(settings);
            
            if (abreSecao)
            {
                Session = client.StartSession();
                Database = Session.Client.GetDatabase(nomeBase);
            }
            else
            {
                Database = client.GetDatabase(nomeBase);
            }
            Collection = Database.GetCollection<T>(collectionName);

            RegisterMapIfNeeded<Paciente>(collectionName);
            RegisterMapIfNeeded<Pessoa>(collectionName);
            RegisterMapIfNeeded<Usuario>(collectionName);
            RegisterMapIfNeeded<Medico>(collectionName);
            RegisterMapIfNeeded<Convenio>(collectionName);
            RegisterMapIfNeeded<Especialidade>(collectionName);
            RegisterMapIfNeeded<ConfiguracaoAgenda>(collectionName);
        }


        // Check to see if map is registered before registering class map
        // This is for the sake of the polymorphic types that we are using so Mongo knows how to deserialize
        public void RegisterMapIfNeeded<TClass>(string collectionName)
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(TClass)))
            {
                BsonClassMap.RegisterClassMap<TClass>();

                var partition = new BsonDocument {
                    {"shardCollection", $"{Database.DatabaseNamespace.DatabaseName}.{collectionName}"},
                    {"key", new BsonDocument {{$"{collectionName}Id", "hashed"}}}
                };
                var command = new BsonDocumentCommand<BsonDocument>(partition);
                Database.RunCommandAsync(command);
            }
        }

        public void Dispose()
        {
            Session?.Dispose();
        }
    }
}
