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

        [BsonElement("ClinicasId")]
        public List<string> ClinicasId { get; set; }
        [BsonIgnore]
        public List<Clinica> Clinicas { get; set; }

        [BsonElement("ConfiguracaoAgendaDiasId")]
        public List<string> ConfiguracaoAgendaDiasId { get; set; }

        [BsonIgnore]
        public List<ConfiguracaoAgendaDias> ConfiguracaoAgendaDias { get; set; }

        [BsonElement("ConfiguracaoMinutosAgenda")]
        public EConfiguracaoMinutosAgenda ConfiguracaoMinutosAgenda { get; set; }


    }
}

