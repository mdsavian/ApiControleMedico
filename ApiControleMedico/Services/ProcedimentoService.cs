using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ApiControleMedico.Modelos;
using ApiControleMedico.Repositorio;
using MongoDB.Driver;

namespace ApiControleMedico.Services
{
    public class ProcedimentoService
    {
        protected readonly DbContexto<Procedimento> ContextoProcedimentos;
        protected readonly EntidadeNegocio<Procedimento> ProcedimentoNegocio = new EntidadeNegocio<Procedimento>();

        public ProcedimentoService()
        {
            ContextoProcedimentos = new DbContexto<Procedimento>("procedimento");
        }

        public async Task<IEnumerable<Procedimento>> GetAllAsync()
        {
            var procedimentos = await ProcedimentoNegocio.GetAllAsync(ContextoProcedimentos.Collection);
            return procedimentos;
        }

        public Task<Procedimento> GetOneAsync(string id)
        {
            return ProcedimentoNegocio.GetOneAsync(ContextoProcedimentos.Collection, id);
        }

        public async Task<Procedimento> SaveOneAsync(Procedimento context)
        {
            await ProcedimentoNegocio.SaveOneAsync(ContextoProcedimentos.Collection, context);

            return context;
        }

        public Task<bool> RemoveOneAsync(string id)
        {
            return ProcedimentoNegocio.RemoveOneAsync(ContextoProcedimentos.Collection, id);
        }

        public async void SaveManyAsync(Collection<Procedimento> procedimentos)
        {
            foreach (var procedimento in procedimentos)
            {
                if (ContextoProcedimentos.Collection.Find(c => c.Descricao == procedimento.Descricao)
                        .FirstOrDefault() != null)
                    continue;

                await ProcedimentoNegocio.SaveOneAsync(ContextoProcedimentos.Collection, procedimento);
            }
        }
    }
}