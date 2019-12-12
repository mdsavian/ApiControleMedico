using System.Collections.Generic;
using ApiControleMedico.Modelos.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace ApiControleMedico.Modelos
{
    public class ConfiguracaoAgenda : Entidade
    {
        [BsonElement("DiasNaoConfigurados")]
        public int[] DiasNaoConfigurados { get; set; }
        [BsonElement("PrimeiroHorario")]
        public string PrimeiroHorario { get; set; }
        [BsonElement("UltimoHorario")]
        public string UltimoHorario { get; set; }

        [BsonElement("MedicoId")]
        public string MedicoId { get; set; }

        [BsonElement("ClinicaId")]
        public string ClinicaId { get; set; }

        [BsonElement("ConfiguracaoAgendaDias")]
        public List<ConfiguracaoAgendaDias> ConfiguracaoAgendaDias { get; set; }

        [BsonElement("ConfiguracaoMinutosAgenda")]
        public EConfiguracaoMinutosAgenda ConfiguracaoMinutosAgenda { get; set; }


    }
}

