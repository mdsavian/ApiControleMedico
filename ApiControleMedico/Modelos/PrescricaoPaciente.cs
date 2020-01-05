using System;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace ApiControleMedico.Modelos
{
    [Serializable, JsonObject]
    [BsonDiscriminator(Required = true)]
    [BsonKnownTypes(typeof(PrescricaoPaciente))]

    public class PrescricaoPaciente : Entidade
    {
        [BsonElement("MedicoId")]
        public string MedicoId { get; set; }

        [BsonElement("PacienteId")]
        public string PacienteId { get; set; }

        [BsonElement("UsuarioId")]
        public string UsuarioId { get; set; }

        [BsonElement("Data")]
        public DateTime Data { get; set; }

        [BsonElement("Descricao")]
        public string Descricao { get; set; }

    }
}
