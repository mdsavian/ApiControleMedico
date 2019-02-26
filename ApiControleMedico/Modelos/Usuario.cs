﻿using System;
using ApiControleMedico.Modelos.Enums;
using MongoDB.Bson;
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

        [BsonElement("PermissaoAdministrador")]
        public bool PermissaoAdministrador { get; set; }

        [BsonElement("VisualizaValoresRelatorios")]
        public bool VisualizaValoresRelatorios { get; set; }

        [BsonElement("Token")]
        public string Token{ get; set; }

        [BsonElement("MedicoId")]
        public string MedicoId{ get; set; }

        [BsonElement("Ativo")]
        public bool Ativo { get; set; }
    }
}
