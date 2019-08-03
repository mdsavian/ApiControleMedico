using System;
using System.Collections.Generic;
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
        [BsonElement("DataAgendamento")] public string DataAgendamento { get; set; }

        [BsonElement("HoraInicial")] public string HoraInicial { get; set; }

        [BsonElement("HoraFinal")] public string HoraFinal { get; set; }

        [BsonElement("Observacao")] public string Observacao { get; set; }

        [BsonElement("PacienteId")] public string PacienteId { get; set; }

        [BsonElement("MedicoId ")] public string MedicoId { get; set; }

        [BsonElement("TipoAgendamento")] public ETipoAgendamento TipoAgendamento { get; set; }

        [BsonElement("SituacaoAgendamento")] public ESituacaoAgendamento SituacaoAgendamento { get; set; }

        [BsonElement("ExameId")] public string ExameId { get; set; }

        [BsonElement("LocalId")] public string LocalId { get; set; }

        [BsonElement("CirurgiaId")] public string CirurgiaId { get; set; }

        [BsonElement("ConvenioId")] public string ConvenioId { get; set; }

        [BsonElement("ProcedimentoId")] public string ProcedimentoId { get; set; }

        [BsonElement("CorFundo")] public string CorFundo { get; set; }

        [BsonElement("CorLetra")] public string CorLetra { get; set; }

        [BsonElement("ClinicasId")] public List<string> ClinicasId { get; set; }

        [BsonElement("Medico")] public Medico Medico { get; set; }
        [BsonElement("Paciente")] public Paciente Paciente { get; set; }
        [BsonElement("Clinicas")] public List<Clinica> Clinicas { get; set; }
        [BsonElement("Exame")] public Exame Exame { get; set; }
        [BsonElement("Local")] public Local Local { get; set; }
        [BsonElement("Cirurgia")] public Cirurgia Cirurgia { get; set; }
        [BsonElement("Convenio")] public Convenio Convenio { get; set; }
        [BsonElement("Procedimento")] public Procedimento Procedimento { get; set; }


    }
}
