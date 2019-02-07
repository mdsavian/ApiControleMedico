using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace ApiControleMedico.Modelos.NaoPersistidos
{
    public class DadosRelatorio
    {
        private DateTime Data { get; set; }
        private long Documento { get; set; }
        private long Carteira { get; set; }
        private string Beneficiario { get; set; }
        private string TipoPlano { get; set; }
        private long CodigoMovimento { get; set; }
        private string Servico { get; set; }
        private decimal Quantidade { get; set; }
        private decimal ValorProduto { get; set; }
        private decimal ValorParticipacao { get; set; }
    }
}
