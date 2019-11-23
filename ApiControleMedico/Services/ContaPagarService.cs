using System;
using System.Collections.Generic;
using System.Linq;
using ApiControleMedico.Modelos;
using ApiControleMedico.Repositorio;
using MongoDB.Driver;
using ApiControleMedico.Uteis;

namespace ApiControleMedico.Services
{
    public class ContaPagarService
    {
        protected readonly DbContexto<ContaPagar> ContextoContasPagar;
        protected readonly EntidadeNegocio<ContaPagar> ContaPagarNegocio = new EntidadeNegocio<ContaPagar>();

        public ContaPagarService()
        {
            ContextoContasPagar = new DbContexto<ContaPagar>("contaPagar");
        }

        public IEnumerable<ContaPagar> GetAll()
        {
            var contaPagars = ContaPagarNegocio.GetAll(ContextoContasPagar.Collection);
            return contaPagars;
        }

        public ContaPagar GetOne(string id)
        {
            return ContaPagarNegocio.GetOne(ContextoContasPagar.Collection, id);
        }

        public ContaPagar SaveOne(ContaPagar context)
        {
            ContaPagarNegocio.SaveOne(ContextoContasPagar.Collection, context);

            return context;
        }

        public List<ContaPagar> buscarContaPagarPorFornecedor(string fornecedorId)
        {
            return ContextoContasPagar.Collection.AsQueryable().Where(c => c.FornecedorId == fornecedorId).Take(5).ToList();
        }

        public bool RemoveOne(string id)
        {
            return ContaPagarNegocio.RemoveOne(ContextoContasPagar.Collection, id);
        }

        internal List<ContaPagar> TodosPorPeriodo(DateTime primeiroDiaMes, DateTime dataHoje, string medicoId)
        {
            return ContextoContasPagar.Collection.Find(c => c.DataEmissao >= primeiroDiaMes && c.DataEmissao <= dataHoje && (medicoId.IsNullOrWhiteSpace() || c.MedicoId == medicoId)).ToList();
        }
    }
}