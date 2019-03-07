using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiControleMedico.Modelos.NaoPersistidos
{
    public class Endereco
    {
        public string Rua { get; set; }
        public string Bairro { get; set; }
        public string Cep { get; set; }
        public string Cidade { get; set; }
        public string Complemento { get; set; }
        public string Uf { get; set; }
    }
}
