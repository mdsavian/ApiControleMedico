using System;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace ApiControleMedico.Modelos
{
    [Serializable, JsonObject]
    [BsonDiscriminator(Required = true)]
    [BsonKnownTypes(typeof(Funcionario))]

    public class Funcionario : Pessoa
    {
        [BsonElement("Oficio")]
        public Oficio Oficio { get; set; }

        [BsonElement("DataAdmissao")]
        public DateTime DataAdmissao { get; set; }

        [BsonElement("DataDemissao")]
        public DateTime DataDemissao { get; set; }

        [BsonElement("Usuario")]
        public Usuario Usuario { get; set; }

        [BsonElement("PermissaoAdministrador")]
        public bool PermissaoAdministrador { get; set; }

        [BsonElement("VisualizaValoresRelatorios")]
        public bool VisualizaValoresRelatorios { get; set; }

    }
}
