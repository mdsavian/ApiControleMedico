﻿using System;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace ApiControleMedico.Modelos
{
    [Serializable, JsonObject]
    [BsonDiscriminator(Required = true)]
    [BsonKnownTypes(typeof(Convenio))]
    public class Convenio : Entidade
    {
        [BsonElement("NomeConvenio")]
        public string NomeConvenio { get; set; }
        [BsonElement("DiasRetorno")]
        public int DiasRetorno { get; set; }

    }
}
