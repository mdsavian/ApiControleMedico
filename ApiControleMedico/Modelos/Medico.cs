using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace ApiControleMedico.Modelos
{
    public class Medico:Pessoa
    {
        [BsonElement("Crm")]
        public string Crm { get; set; }

        [BsonElement("Convenios")]
        public List<Convenio> Convenios { get; set; }
    }
}
