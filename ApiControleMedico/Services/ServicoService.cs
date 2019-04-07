using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ApiControleMedico.Modelos;
using ApiControleMedico.Repositorio;
using MongoDB.Driver;

namespace ApiControleMedico.Services
{
    public class ServicoService
    {
        protected readonly DbContexto<Servico> ContextoServicos;
        protected readonly EntidadeNegocio<Servico> ServicoNegocio = new EntidadeNegocio<Servico>();

        public ServicoService()
        {
            ContextoServicos = new DbContexto<Servico>("servico");
        }

        public async Task<IEnumerable<Servico>> GetAllAsync()
        {
            var servicos = await ServicoNegocio.GetAllAsync(ContextoServicos.Collection);
            return servicos;
        }

        public Task<Servico> GetOneAsync(string id)
        {
            return ServicoNegocio.GetOneAsync(ContextoServicos.Collection, id);
        }

        public async Task<Servico> SaveOneAsync(Servico context)
        {
            await ServicoNegocio.SaveOneAsync(ContextoServicos.Collection, context);

            return context;
        }

        public Task<bool> RemoveOneAsync(string id)
        {
            return ServicoNegocio.RemoveOneAsync(ContextoServicos.Collection, id);
        }

        public async void SaveManyAsync(Collection<Servico> servicos)
        {
            foreach (var servico in servicos)
            {
                if (ContextoServicos.Collection.Find(c => c.Descricao == servico.Descricao)
                        .FirstOrDefault() != null)
                    continue;

                await ServicoNegocio.SaveOneAsync(ContextoServicos.Collection, servico);
            }
        }
    }
}