using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ApiControleMedico.Modelos;
using ApiControleMedico.Repositorio;
using MongoDB.Driver;

namespace ApiControleMedico.Services
{
    public class LocalService
    {
        protected readonly DbContexto<Local> ContextoLocals;
        protected readonly EntidadeNegocio<Local> LocalNegocio = new EntidadeNegocio<Local>();

        public LocalService()
        {
            ContextoLocals = new DbContexto<Local>("local");
        }

        public async Task<IEnumerable<Local>> GetAllAsync()
        {
            var locals = await LocalNegocio.GetAllAsync(ContextoLocals.Collection);
            return locals;
        }

        public Task<Local> GetOneAsync(string id)
        {
            return LocalNegocio.GetOneAsync(ContextoLocals.Collection, id);
        }

        public async Task<Local> SaveOneAsync(Local context)
        {
            await LocalNegocio.SaveOneAsync(ContextoLocals.Collection, context);

            return context;
        }

        public Task<bool> RemoveOneAsync(string id)
        {
            return LocalNegocio.RemoveOneAsync(ContextoLocals.Collection, id);
        }

        public async void SaveManyAsync(Collection<Local> locals)
        {
            foreach (var local in locals)
            {
                if (ContextoLocals.Collection.Find(c => c.Descricao == local.Descricao)
                        .FirstOrDefault() != null)
                    continue;

                await LocalNegocio.SaveOneAsync(ContextoLocals.Collection, local);
            }
        }
    }
}