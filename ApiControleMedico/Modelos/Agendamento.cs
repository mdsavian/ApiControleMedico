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
        [BsonElement("DataAgendamento")] public DateTime DataAgendamento { get; set; }

        [BsonElement("HoraInicial")] public string HoraInicial { get; set; }

        [BsonElement("HoraFinal")] public string HoraFinal { get; set; }

        [BsonElement("HoraInicialAtendimento")] public string HoraInicialAtendimento { get; set; }

        [BsonElement("HoraFinalAtendimento")] public string HoraFinalAtendimento { get; set; }

        [BsonElement("Observacao")] public string Observacao { get; set; }

        [BsonElement("PacienteId")] public string PacienteId { get; set; }

        [BsonElement("FuncionarioId")] public string FuncionarioId { get; set; }

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

        [BsonElement("ClinicaId")] public string ClinicaId { get; set; }

        [BsonElement("ContemPagamentos")] public bool ContemPagamentos { get; set; }

        [BsonElement("Encaixado")] public bool Encaixado { get; set; }

        [BsonElement("Pagamentos")] public List<AgendamentoPagamento> Pagamentos{ get; set; }

        [BsonElement("DataInicioAtendimento")] public DateTime DataInicioAtendimento { get; set; }
        [BsonElement("DescricaoAtendimento")] public string DescricaoAtendimento { get; set; }


        [BsonIgnore] public string TipoAgendamentoDescricao { get; set; }


        [BsonIgnore] public Medico Medico { get; set; }
        [BsonIgnore] public Paciente Paciente { get; set; }
        [BsonIgnore] public Exame Exame { get; set; }
        [BsonIgnore] public Local Local { get; set; }
        [BsonIgnore] public Cirurgia Cirurgia { get; set; }
        [BsonIgnore] public Clinica Clinica { get; set; }
        [BsonIgnore] public Convenio Convenio { get; set; }
        [BsonIgnore] public Procedimento Procedimento { get; set; }


    }
}
