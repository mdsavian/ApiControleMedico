using MongoDB.Bson.Serialization.Attributes;

namespace ApiControleMedico.Modelos
{
    public class Clinica:Entidade
    {
        [BsonElement("Ativo")]
        public bool Ativo { get; set; }
        [BsonElement("RazaoSocial")]
        public string RazaoSocial { get; set; }
        [BsonElement("NomeFantasia")]
        public string NomeFantasia { get; set; }
        //[BsonElement("Imagem")]
        //public byte[] Imagem { get; set; }
        [BsonElement("Telefone")]
        public string Telefone { get; set; }
        [BsonElement("Endereco")]
        public string Endereco { get; set; }
        [BsonElement("Bairro")]
        public string Bairro { get; set; }
        [BsonElement("Cidade")]
        public string Cidade { get; set; }
        [BsonElement("Uf")]
        public string Uf { get; set; }
        [BsonElement("Cep")]
        public string Cep { get; set; }
        [BsonElement("Numero")]
        public string Numero { get; set; }
        [BsonElement("Complemento")]
        public string Complemento { get; set; }
        [BsonElement("Cnpj")]
        public string Cnpj { get; set; }
        [BsonElement("Ie")]
        public string Ie { get; set; }
        [BsonElement("Im")]
        public string Im { get; set; }
    }
}