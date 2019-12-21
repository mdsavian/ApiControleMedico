using System;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace ApiControleMedico.Modelos
{
    [Serializable, JsonObject]
    [BsonDiscriminator(Required = true)]
    [BsonKnownTypes(typeof(AnexoProntuario))]

    public class AnexoProntuario : Entidade
    {
        [BsonElement("ExtensaoArquivo")]
        public string ExtensaoArquivo { get; set; }

        [BsonElement("NomeArquivo")]
        public string NomeArquivo { get; set; }

        [BsonElement("ContentType")]
        public string ContentType { get; set; }

    }
}
