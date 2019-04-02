using System;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace ApiControleMedico.Modelos
{
    [Serializable, JsonObject]
    [BsonDiscriminator(Required = true)]
    [BsonKnownTypes(typeof(Especialidade))]

    public class Especialidade : Entidade
    {
        [BsonElement("Descricao")]
        public string Descricao { get; set; }

    }
}
