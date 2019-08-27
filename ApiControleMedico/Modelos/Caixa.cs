using MongoDB.Bson.Serialization.Attributes;

namespace ApiControleMedico.Modelos
{
    public class Caixa:Entidade
    {
        [BsonElement("ClinicaId")]
        public string ClinicaId { get; set; }

        [BsonElement("FuncionarioId")]
        public string FuncionarioId{ get; set; }

        [BsonElement("DataAbertura")]
        public string DataAbertura { get; set; }

        [BsonElement("HoraAbertura")]
        public string HoraAbertura { get; set; }

        [BsonElement("DataFechamento")]
        public string DataFechamento { get; set; }

        [BsonElement("HoraFechamento")]
        public string HoraFechamento { get; set; }

        [BsonElement("TrocoAbertura")]
        public decimal TrocoAbertura { get; set; }

        [BsonElement("TrocoFechamento")]
        public decimal TrocoFechamento { get; set; }

        [BsonElement("UsuarioFechamentoId")]
        public string UsuarioFechamentoId;

        [BsonElement("UsuarioAberturaId")]
        public string UsuarioAberturaId;
    }
}