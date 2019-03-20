using System;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace ApiControleMedico.Modelos
{
    [Serializable, JsonObject]
    [BsonDiscriminator(Required = true)]
    [BsonKnownTypes(typeof(Oficio))]

    public class Oficio : Entidade
    {
        [BsonElement("Descricao")]
        public string Descricao { get; set; }

    }
}
