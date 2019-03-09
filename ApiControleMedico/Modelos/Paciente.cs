using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace ApiControleMedico.Modelos
{
    [Serializable, JsonObject]
    [BsonDiscriminator(Required = true)]
    [BsonKnownTypes(typeof(Paciente))]
    public class Paciente : Pessoa
    {
        //Primeiro folder
        [BsonElement("NomeConjugue")]
        public string NomeConjugue { get; set; }
        [BsonElement("NomePai")]
        public string NomePai { get; set; }
        [BsonElement("NomeMae")]
        public string NomeMae { get; set; }
        [BsonElement("EstadoCivil")]
        public string EstadoCivil { get; set; }
        [BsonElement("TipoSanguineo")]
        public string TipoSanguineo { get; set; }
        [BsonElement("Ocupacao")]
        public string Ocupacao { get; set; } 
        //segundo folder
        [BsonElement("Telefone")]
        public string Telefone { get; set; }
        [BsonElement("AceitaReceberSms")]
        public bool AceitaReceberSms { get; set; } 
        [BsonElement("Responsavel")]
        public string Responsavel { get; set; }

        //quarto folder
        [BsonElement("Convenio")]
        public Convenio Convenio { get; set; } 
        [BsonElement("NumeroCartao")]
        public long NumeroCartao { get; set; }
        [BsonElement("CartaoNacionalSaude")]
        public long CartaoNacionalSaude { get; set; }
        [BsonElement("DataValidadeCartao")]
        public DateTime DataValidadeCartao { get; set; }
        [BsonElement("TipoPlano")]
        public string TipoPlano { get; set; }
         

        public Paciente()
        {
            this.Ativo = true;
            this.AceitaReceberSms = true;

        }


    }
}
