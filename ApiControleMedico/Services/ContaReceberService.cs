using System.Collections.Generic;
using System.Linq;
using ApiControleMedico.Modelos;
using ApiControleMedico.Repositorio;
using MongoDB.Driver;

namespace ApiControleMedico.Services
{
    public class ContaPagarService
    {
        protected readonly DbContexto<ContaPagar> ContextoContaPagars;
        protected readonly EntidadeNegocio<ContaPagar> ContaPagarNegocio = new EntidadeNegocio<ContaPagar>();

        public ContaPagarService()
        {
            ContextoContaPagars = new DbContexto<ContaPagar>("contaPagar");
        }

        public IEnumerable<ContaPagar> GetAll()
        {
            var contaPagars = ContaPagarNegocio.GetAll(ContextoContaPagars.Collection);
            return contaPagars;
        }

        public ContaPagar GetOne(string id)
        {
            return ContaPagarNegocio.GetOne(ContextoContaPagars.Collection, id);
        }

        public ContaPagar SaveOne(ContaPagar context)
        {
            ContaPagarNegocio.SaveOne(ContextoContaPagars.Collection, context);

            return context;
        }

        public List<ContaPagar> buscarContaPagarPorFornecedor(string fornecedorId)
        {
            return ContextoContaPagars.Collection.AsQueryable().Where(c => c.FornecedorId == fornecedorId).Take(5).ToList();
        }

        public bool RemoveOne(string id)
        {
            return ContaPagarNegocio.RemoveOne(ContextoContaPagars.Collection, id);
        }
    }
}