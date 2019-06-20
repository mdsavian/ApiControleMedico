using System;
using ApiControleMedico.Modelos.Enums;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace ApiControleMedico.Modelos
{
    [Serializable, JsonObject]
    [BsonDiscriminator(Required = true)]
    [BsonKnownTypes(typeof(Agendamento))]

    public class Agendamento : Entidade
    {
        [BsonElement("DataAgendamento")]
        public string DataAgendamento { get; set; }
        [BsonElement("HoraInicial")]
        public string HoraInicial { get; set; }
        [BsonElement("HoraFinal")]
        public string HoraFinal { get; set; }
        [BsonElement("Observacao")]
        public string Observacao { get; set; }
        [BsonElement("Paciente")]
        public Paciente Paciente { get; set; }
        [BsonElement("Medico")]
        public Medico Medico { get; set; }
        [BsonElement("TipoAgendamento")]
        public ETipoAgendamento TipoAgendamento { get; set; }
        [BsonElement("SituacaoAgendamento")]
        public ESituacaoAgendamento SituacaoAgendamento{ get; set; }
        [BsonElement("Exame")]
        public Exame Exame { get; set; }
        [BsonElement("Local")]
        public Local Local { get; set; }
        [BsonElement("Cirurgia")]
        public Cirurgia Cirurgia { get; set; }
        [BsonElement("Convenio")]
        public Convenio Convenio { get; set; }
        [BsonElement("Procedimento")]
        public Procedimento Procedimento { get; set; }
        [BsonElement("CorFundo")]
        public string CorFundo{ get; set; }
        [BsonElement("CorLetra")]
        public string CorLetra { get; set; }

    }
}
