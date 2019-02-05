using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiControleMedico.Modelos.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ApiControleMedico.Modelos
{
    public class Medico:Pessoa
    {
        [BsonElement("Crm")]
        public string Crm { get; set; }
    }
}
