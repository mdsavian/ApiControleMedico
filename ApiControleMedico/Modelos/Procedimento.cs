using System;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace ApiControleMedico.Modelos
{
    [Serializable, JsonObject]
    [BsonDiscriminator(Required = true)]
    [BsonKnownTypes(typeof(Procedimento))]

    public class Procedimento : Entidade
    {
        [BsonElement("Descricao")]
        public string Descricao { get; set; }

        [BsonElement("CorFundo")]
        public string CorFundo { get; set; }

        [BsonElement("CorLetra")]
        public string CorLetra { get; set; }

        [BsonElement("ObrigaPaciente")]
        public bool ObrigaPaciente { get; set; }

    }
}
