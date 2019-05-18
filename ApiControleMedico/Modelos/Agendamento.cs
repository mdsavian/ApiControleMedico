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
        [BsonElement("DataAgendamentoInicial")]
        public DateTime DataAgendamentoInicial { get; set; }
        [BsonElement("DataAgendamentoFinal")]
        public DateTime DataAgendamentoFinal { get; set; }
        [BsonElement("Observacao")]
        public string Observacao { get; set; }
        [BsonElement("Paciente")]
        public Paciente Paciente { get; set; }
        [BsonElement("Medico")]
        public Medico Medico { get; set; }
        [BsonElement("TipoAgendamento")]
        public ETipoAgendamento TipoAgendamento { get; set; }
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
        [BsonElement("Cor")]
        public Procedimento Cor{ get; set; }

    }
}
