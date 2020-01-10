using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace ApiControleMedico.Modelos
{
    public class Medico:Pessoa
    {
        [BsonElement("Crm")]
        public string Crm { get; set; }

        [BsonElement("ConveniosId")]
        public List<string> ConveniosId { get; set; }
        
        [BsonElement("UsuarioId")]
        public string UsuarioId { get; set; }

        [BsonElement("EspecialidadeId")]
        public string EspecialidadeId { get; set; }

        //[BsonElement("ConfiguracaoAgendaId")]
        //public string ConfiguracaoAgendaId { get; set; }

        [BsonElement("Administrador")]
        public bool Administrador { get; set; }
        
        [BsonElement("ClinicasId")]
        public List<string> ClinicasId { get; set; }

        [BsonElement("FotoId")]
        public string FotoId { get; set; }

        [BsonElement("TempoRenovarSessao")]
        public int TempoRenovarSessao { get; set; }

        [BsonIgnore]
        public List<Clinica> Clinicas { get; set; }
        [BsonIgnore]
        public List<Convenio> Convenios { get; set; }
        [BsonIgnore]
        public ConfiguracaoAgenda ConfiguracaoAgenda { get; set; }
        [BsonIgnore]
        public Especialidade Especialidade { get; set; }
        [BsonIgnore]
        public Usuario Usuario { get; set; }

    }
}
