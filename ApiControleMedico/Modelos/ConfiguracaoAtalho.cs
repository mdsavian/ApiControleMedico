using MongoDB.Bson.Serialization.Attributes;

namespace ApiControleMedico.Modelos
{
    public class ConfiguracaoAtalho : Entidade
    {
        public ConfiguracaoAtalho()
        {
            Ativo = true;
        }

        [BsonElement("Descricao")]
        public string Descricao { get; set; }
        [BsonElement("Imagem")]
        public string Imagem { get; set; }
        [BsonElement("SpanClass")]
        public string SpanClass { get; set; }
        [BsonElement("BtnClass")]
        public string BtnClass { get; set; }
        [BsonElement("Ativo")]
        public bool Ativo { get; set; }
    }
}
