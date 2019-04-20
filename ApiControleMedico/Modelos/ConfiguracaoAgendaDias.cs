using MongoDB.Bson.Serialization.Attributes;

namespace ApiControleMedico.Modelos
{
    public class ConfiguracaoAgendaDias : Entidade
    {
        [BsonElement("Dia")]
        public int Dia { get; set;}

        [BsonElement("Configurado")]
        public bool Configurado { get; set; }

        [BsonElement("PrimeiroHorarioInicial")]
        public string PrimeiroHorarioInicial { get; set; }

        [BsonElement("PrimeiroHorarioFinal")]
        public string PrimeiroHorarioFinal { get; set; }

        [BsonElement("SegundoHorarioInicial")]
        public string SegundoHorarioInicial { get; set; }

        [BsonElement("SegundoHorarioFinal")]
        public string SegundoHorarioFinal { get; set; }

        [BsonElement("HorarioInicioIntervalo")]
        public string HorarioInicioIntervalo { get; set; }

        [BsonElement("HorarioFimIntervalo")]
        public string HorarioFimIntervalo { get; set; }

    }
}

