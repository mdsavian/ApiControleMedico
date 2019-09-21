using MongoDB.Bson.Serialization.Attributes;

namespace ApiControleMedico.Modelos
{
    public class Fornecedor : Pessoa
    {
        [BsonElement("InscricaoMunicipal")]
        public string InscricaoMunicipal { get; set; }
        [BsonElement("InscricaoEstadual")]
        public string InscricaoEstadual { get; set; }
        [BsonElement("RazaoSocial")]
        public string RazaoSocial { get; set; }
        [BsonElement("NomeFantasia")]
        public string NomeFantasia { get; set; }
        [BsonElement("Pais")]
        public string Pais { get; set; }
        [BsonElement("Telefone")]
        public string Telefone { get; set; }
    }
}
