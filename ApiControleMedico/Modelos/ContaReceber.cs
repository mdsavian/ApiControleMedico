using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace ApiControleMedico.Modelos
{
    public class ContaReceber : Entidade
    {
        [BsonElement("ClinicaId")]
        public string ClinicaId { get; set; }
        [BsonElement("PacienteId")]
        public string PacienteId { get; set; }
        [BsonElement("AgendamentoId")]
        public string AgendamentoId { get; set; }
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
        [BsonElement("TipoContaReceber")]
        public string TipoContaReceber { get; set; }
        [BsonElement("Observacao")]
        public string Observacao { get; set; }
        [BsonElement("Pagamentos")] public List<ContaReceberPagamento> Pagamentos { get; set; }

        [BsonIgnore] public string TipoContaDescricao {get;set;}
    }
}
