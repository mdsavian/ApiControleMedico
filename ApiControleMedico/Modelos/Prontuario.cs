using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace ApiControleMedico.Modelos
{
    [Serializable, JsonObject]
    [BsonDiscriminator(Required = true)]
    [BsonKnownTypes(typeof(Prontuario))]

    public class Prontuario : Entidade
    {
        [BsonElement("Descricao")]
        public string Descricao { get; set; }

        [BsonElement("Anexos")]
        public List<AnexoProntuario> Anexos { get; set; }

        [BsonElement("PacienteId")]
        public string PacienteId { get; set; }




    }
}
