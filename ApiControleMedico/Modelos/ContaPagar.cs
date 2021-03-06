﻿using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace ApiControleMedico.Modelos
{
    public class ContaPagar : Entidade
    {    
        [BsonElement("ClinicaId")]
        public string ClinicaId { get; set; }
        [BsonElement("FornecedorId")]
        public string FornecedorId { get; set; }
        [BsonElement("MedicoId")]
        public string MedicoId { get; set; }
        [BsonElement("UsuarioId")]
        public string UsuarioId { get; set; }
        [BsonElement("DataEmissao")]
        public DateTime DataEmissao { get; set; }
        [BsonElement("DataVencimento")]
        public DateTime DataVencimento { get; set; }
        [BsonElement("JurosMulta")]
        public decimal JurosMulta { get; set; }
        [BsonElement("Desconto")]
        public decimal Desconto { get; set; }
        [BsonElement("NumeroDocumento")]
        public string NumeroDocumento { get; set; }
        [BsonElement("NumeroFatura")]
        public long NumeroFatura { get; set; }
        [BsonElement("Valor")]
        public decimal Valor { get; set; }
        [BsonElement("Saldo")]
        public decimal Saldo { get; set; }
        [BsonElement("ValorTotal")]
        public decimal ValorTotal { get; set; }
        [BsonElement("TipoContaPagar")]
        public int TipoContaPagar { get; set; }
        [BsonElement("Observacao")]
        public string Observacao { get; set; }

        [BsonElement("Pagamentos")] public List<ContaPagarPagamento> Pagamentos { get; set; }
    }
}
