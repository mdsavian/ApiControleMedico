﻿using MongoDB.Bson.Serialization.Attributes;
using System;

namespace ApiControleMedico.Modelos
{
    public class Caixa:Entidade
    {
        [BsonElement("ClinicaId")]
        public string ClinicaId { get; set; }

        [BsonElement("FuncionarioId")]
        public string FuncionarioId{ get; set; }

        [BsonElement("MedicoId ")]
        public string MedicoId { get; set; }

        [BsonElement("DataAbertura")]
        public DateTime DataAbertura { get; set; }

        [BsonElement("HoraAbertura")]
        public string HoraAbertura { get; set; }

        [BsonElement("DataFechamento")]
        public DateTime? DataFechamento { get; set; }

        [BsonElement("HoraFechamento")]
        public string HoraFechamento { get; set; }

        [BsonElement("TrocoAbertura")]
        public decimal TrocoAbertura { get; set; }

        [BsonElement("TrocoFechamento")]
        public decimal TrocoFechamento { get; set; }

        [BsonElement("UsuarioFechamentoId")]
        public string UsuarioFechamentoId { get; set; }

        [BsonElement("UsuarioAberturaId")]
        public string UsuarioAberturaId { get; set; }

        [BsonElement("PessoaId")]
        public string PessoaId { get; set; }

        [BsonElement("Administrativo")]
        public bool Administrativo { get; set; }

    }
}