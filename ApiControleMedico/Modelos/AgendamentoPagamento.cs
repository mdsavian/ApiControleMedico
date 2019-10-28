using ApiControleMedico.Modelos.Enums;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace ApiControleMedico.Modelos
{
    public class AgendamentoPagamento
    {
        [BsonElement("Data")] public DateTime Data { get; set; }

        [BsonElement("CaixaId")] public string CaixaId { get; set; }

        [BsonElement("UsuarioId")] public string UsuarioId { get; set; }

        [BsonElement("FormaPagamentoId")] public string FormaPagamentoId { get; set; }

        [BsonElement("VistaPrazo")] public EVistaPrazo VistaPrazo { get; set; }

        [BsonElement("Parcela")] public int Parcela { get; set; }

        [BsonElement("Valor")] public decimal Valor { get; set; }

    }
}
