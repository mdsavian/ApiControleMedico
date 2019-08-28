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

    public class AgendamentoPagamento : Entidade
    {
        [BsonElement("Data")] public string Data { get; set; }

        [BsonElement("CaixaId")] public string CaixaId { get; set; }

        [BsonElement("UsuarioId")] public string UsuarioId { get; set; }

        [BsonElement("AgendamentoId")] public string AgendamentoId { get; set; }

        [BsonElement("FormaPagamentoId")] public string FormaPagamentoId { get; set; }

        [BsonElement("VistaPrazo")] public EVistaPrazo VistaPrazo { get; set; }

        [BsonElement("Parcela")] public int Parcela { get; set; }

    }
}
