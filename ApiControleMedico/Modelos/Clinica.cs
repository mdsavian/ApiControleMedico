using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace ApiControleMedico.Modelos
{
    public class Clinica
    {
        public string NomeClinica { get; set; }
        public int eitcha { get; set; }
        public ObjectId opa { get; set; }

        //public string EmailClinica{ get; set; }
        //public string Telefone{ get; set; }
        //public string Endereco{ get; set; }
        //public string Cidade{ get; set; }
        //public string Uf{ get; set; }
        //public string Cep{ get; set; }
        //public string Numero{ get; set; }
    }
}
