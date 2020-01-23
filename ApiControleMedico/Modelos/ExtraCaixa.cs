﻿using ApiControleMedico.Modelos.Enums;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace ApiControleMedico.Modelos
{
    public class ExtraCaixa : Entidade
    {
        [BsonElement("ClinicaId")]
        public string ClinicaId { get; set; }

        [BsonElement("UsuarioId")]
        public string UsuarioId { get; set; }

        [BsonElement("CaixaId")]
        public string CaixaId { get; set; }

        [BsonElement("Valor")] public decimal Valor { get; set; }

        [BsonElement("FormaPagamentoId")] public string FormaPagamentoId { get; set; }

        [BsonElement("Data")] public DateTime Data { get; set; }

        [BsonElement("Parcela")] public int Parcela { get; set; }

        [BsonElement("VistaPrazo")] public ETipoExtraCaixa VistaPrazo { get; set; }

        [BsonElement("TipoExtraCaixa")] public int TipoExtraCaixa { get; set; }

    }
}
