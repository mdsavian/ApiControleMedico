using System;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace ApiControleMedico.Modelos
{
    [Serializable, JsonObject]
    [BsonDiscriminator(Required = true)]
    [BsonKnownTypes(typeof(Exame))]

    public class Exame : Entidade
    {
        [BsonElement("Descricao")]
        public string Descricao { get; set; }

        [BsonElement("CorFundo")]
        public string CorFundo { get; set; }

        [BsonElement("CorLetra")]
        public string CorLetra { get; set; }

    }
}
