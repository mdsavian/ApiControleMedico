using System;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace ApiControleMedico.Modelos
{
    [Serializable, JsonObject]
    [BsonDiscriminator(Required = true)]
    [BsonKnownTypes(typeof(ConvenioMedico))]
    public class ConvenioMedico : Entidade
    {
        [BsonElement("ConvenioId")]
        public string ConvenioId { get; set; }
        [BsonElement("MedicoId")]
        public string MedicoId { get; set; }

    }
}
