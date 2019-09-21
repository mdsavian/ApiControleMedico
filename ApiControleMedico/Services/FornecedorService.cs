using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ApiControleMedico.Modelos;
using ApiControleMedico.Repositorio;
using MongoDB.Driver;

namespace ApiControleMedico.Services
{
    public class FornecedorService
    {
        protected readonly DbContexto<Fornecedor> ContextoFornecedors;
        protected readonly EntidadeNegocio<Fornecedor> FornecedorNegocio = new EntidadeNegocio<Fornecedor>();

        public FornecedorService()
        {
            ContextoFornecedors = new DbContexto<Fornecedor>("fornecedor");
        }

        public IEnumerable<Fornecedor> GetAll()
        {
            var fornecedors = FornecedorNegocio.GetAll(ContextoFornecedors.Collection);
            return fornecedors;
        }

        public Fornecedor GetOne(string id)
        {
            return FornecedorNegocio.GetOne(ContextoFornecedors.Collection, id);
        }

        public Fornecedor SaveOne(Fornecedor context)
        {
            FornecedorNegocio.SaveOne(ContextoFornecedors.Collection, context);

            return context;
        }

        public bool RemoveOne(string id)
        {
            return FornecedorNegocio.RemoveOne(ContextoFornecedors.Collection, id);
        }
    }
}