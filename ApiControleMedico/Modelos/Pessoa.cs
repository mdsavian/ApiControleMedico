using System;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace ApiControleMedico.Modelos
{
    [Serializable, JsonObject]
    [BsonDiscriminator(Required = true)]
    [BsonKnownTypes(typeof(Pessoa))]
    public class Pessoa : Entidade
    {
        [BsonElement("NomeCompleto")]
        public string NomeCompleto { get; set; }
        [BsonElement("DataNascimento")]
        public DateTime DataNascimento { get; set; }
        [BsonElement("Rg")]
        public string Rg { get; set; }
        [BsonElement("Cpf")]
        public string Cpf { get; set; }
        [BsonElement("Genero")]
        public int Genero { get; set; }
        [BsonElement("Celular")]
        public string Celular { get; set; }
        [BsonElement("Email")]
        public string Email { get; set; }
        [BsonElement("Ativo")]
        public bool Ativo { get; set; }


        //terceiro folder
        [BsonElement("Cep")]
        public string Cep { get; set; }
        [BsonElement("Endereco")]
        public string Endereco { get; set; }
        [BsonElement("Numero")]
        public string Numero { get; set; }
        [BsonElement("Complemento")]
        public string Complemento { get; set; }
        [BsonElement("Bairro")]
        public string Bairro { get; set; }
        [BsonElement("Cidade")]
        public string Cidade { get; set; }
        [BsonElement("Uf")]
        public string Uf { get; set; }
    }
}
