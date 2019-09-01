using System;
using ApiControleMedico.Modelos.Enums;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace ApiControleMedico.Modelos
{
    [Serializable, JsonObject]
    [BsonDiscriminator(Required = true)]
    [BsonKnownTypes(typeof(FormaDePagamento))]

    public class FormaDePagamento : Entidade
    {
        [BsonElement("Descricao")]
        public string Descricao { get; set; }

        [BsonElement("DiasRecebimento")]
        public int DiasRecebimento { get; set; }
                
        [BsonElement("TipoPagamento")]
        public EVistaPrazo TipoPagamento { get; set; }
    }
}
