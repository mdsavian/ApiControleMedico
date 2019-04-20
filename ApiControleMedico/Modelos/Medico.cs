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

        [BsonElement("Usuario")]
        public Usuario Usuario { get; set; }

        [BsonElement("Especialidade")]
        public Especialidade Especialidade { get; set; }

        [BsonElement("ConfiguracaoAgenda")]
        public ConfiguracaoAgenda ConfiguracaoAgenda { get; set; }

    }
}
