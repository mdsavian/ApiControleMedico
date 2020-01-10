using System;
using ApiControleMedico.Modelos.Enums;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace ApiControleMedico.Modelos
{

    [Serializable, JsonObject]
    [BsonDiscriminator(Required = true)]
    [BsonKnownTypes(typeof(Usuario))]
    public class Usuario : Entidade
    {
        [BsonElement("TipoUsuario")]
        public ETipoUsuario TipoUsuario { get; set; }

        [BsonElement("Login")]
        [BsonDefaultValue("")]
        public string Login { get; set; }

        [BsonElement("Senha")]
        [BsonDefaultValue("")]
        public string Senha { get; set; }

        [BsonElement("UltimoLogin")]
        public string UltimoLogin{ get; set; }

        [BsonElement("Ativo")]
        public bool Ativo { get; set; }

        [BsonElement("MedicoId")]
        public string MedicoId { get; set; }

        [BsonElement("FuncionarioId")]
        public string FuncionarioId { get; set; }

        [BsonElement("SessaoAtiva")]
        public bool SessaoAtiva { get; set; }

        [BsonIgnore]
        public bool SenhaPadrao { get; set; }

        [BsonIgnore]
        public Medico Medico { get; set; }

        [BsonIgnore]
        public int TempoRenovarSessao{ get; set; }

        [BsonIgnore]
        public Funcionario Funcionario { get; set; }

        //[BsonElement("ClinicasId")]
        //public List<string> ClinicasId { get; set; }
        //public List<Clinica> Clinicas { get; set; }
    }
}
