using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace ApiControleMedico.Modelos
{
    [Serializable, JsonObject]
    [BsonDiscriminator(Required = true)]
    [BsonKnownTypes(typeof(Funcionario))]

    public class Funcionario : Pessoa
    {
        [BsonElement("OficioId")]
        public string OficioId { get; set; }

        [BsonIgnore]
        public Oficio Oficio { get; set; }

        [BsonElement("DataAdmissao")]
        public DateTime? DataAdmissao { get; set; }

        [BsonElement("DataDemissao")]
        public DateTime? DataDemissao { get; set; }

        [BsonElement("UsuarioId")]
        public string UsuarioId { get; set; }

        [BsonIgnore]
        public Usuario Usuario { get; set; }

        [BsonElement("PermissaoAdministrador")]
        public bool PermissaoAdministrador { get; set; }

        [BsonElement("VisualizaValoresRelatorios")]
        public bool VisualizaValoresRelatorios { get; set; }

        [BsonElement("VisualizaAgenda")]
        public bool VisualizaAgenda { get; set; }

        [BsonElement("ClinicasId")]
        public List<string> ClinicasId { get; set; }

        [BsonIgnore]
        public List<Clinica> Clinicas { get; set; }

        [BsonElement("MedicosId")]
        public List<string> MedicosId { get; set; }

        [BsonIgnore]
        public List<Medico> Medicos { get; set; }

        [BsonElement("FotoId")]
        public string FotoId { get; set; }

    }
}
