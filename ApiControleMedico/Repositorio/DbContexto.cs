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
            if (abreSecao)
            {
                Session = client.StartSession();
                Database = Session.Client.GetDatabase("ControleMedicoDb");
            }
            else
            {
                Database = client.GetDatabase("ControleMedicoDb");
            }
            Collection = Database.GetCollection<T>(collectionName);

            RegisterMapIfNeeded<Paciente>(collectionName);
            RegisterMapIfNeeded<Pessoa>(collectionName);
            RegisterMapIfNeeded<Usuario>(collectionName);
            RegisterMapIfNeeded<Medico>(collectionName);
            RegisterMapIfNeeded<Convenio>(collectionName);
            RegisterMapIfNeeded<Especialidade>(collectionName);

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
