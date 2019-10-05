using ApiControleMedico.Modelos.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace ApiControleMedico.Modelos
{
    public class ContaPagarPagamento
    {    
        [BsonElement("DataPagamento")]
        public string DataPagamento { get; set; }

        [BsonElement("Valor")]
        public decimal Valor { get; set; }
        [BsonElement("FormaPagamentoId")]
        public string FormaPagamentoId { get; set; }
        [BsonElement("UsuarioId")]
        public string UsuarioId { get; set; }
        [BsonElement("VistaPrazo")]
        public EVistaPrazo VistaPrazo { get; set; }
        [BsonElement("Parcela")]
        public int Parcela { get; set; }
        [BsonElement("Codigo")]
        public int Codigo{ get; set; }




    }
}
