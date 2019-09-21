using MongoDB.Bson.Serialization.Attributes;

namespace ApiControleMedico.Modelos
{
    public class Pais : Entidade
    {
        [BsonElement("Descricao")]
        public string Descricao { get; set; }

        [BsonElement("Codigo")]
        public int Codigo { get; set; }
    }
}
