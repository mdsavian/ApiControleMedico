using System;

namespace ApiControleMedico.Modelos.NaoPersistidos
{
    public class DadosRelatorioUnimed
    {
        public DateTime Data { get; set; }
        public long Documento { get; set; }
        public long Carteira { get; set; }
        public string Beneficiario { get; set; }
        public string TipoPlano { get; set; }
        public long CodigoMovimento { get; set; }
        public string Servico { get; set; }
        public decimal Quantidade { get; set; }
        public decimal ValorProduto { get; set; }
        public decimal ValorParticipacao { get; set; }
        public decimal PrevPagamento { get; set; }
        public string Convenio { get; set; }
    }
}
