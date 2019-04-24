using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ApiControleMedico.Modelos;
using ApiControleMedico.Repositorio;
using MongoDB.Driver;

namespace ApiControleMedico.Services
{
    public class CirurgiaService
    {
        protected readonly DbContexto<Cirurgia> ContextoCirurgias;
        protected readonly EntidadeNegocio<Cirurgia> CirurgiaNegocio = new EntidadeNegocio<Cirurgia>();

        public CirurgiaService()
        {
            ContextoCirurgias = new DbContexto<Cirurgia>("cirurgia");
        }

        public async Task<IEnumerable<Cirurgia>> GetAllAsync()
        {
            var cirurgias = await CirurgiaNegocio.GetAllAsync(ContextoCirurgias.Collection);
            return cirurgias;
        }

        public Task<Cirurgia> GetOneAsync(string id)
        {
            return CirurgiaNegocio.GetOneAsync(ContextoCirurgias.Collection, id);
        }

        public async Task<Cirurgia> SaveOneAsync(Cirurgia context)
        {
            await CirurgiaNegocio.SaveOneAsync(ContextoCirurgias.Collection, context);

            return context;
        }

        public Task<bool> RemoveOneAsync(string id)
        {
            return CirurgiaNegocio.RemoveOneAsync(ContextoCirurgias.Collection, id);
        }

        public async void SaveManyAsync(Collection<Cirurgia> cirurgias)
        {
            foreach (var cirurgia in cirurgias)
            {
                if (ContextoCirurgias.Collection.Find(c => c.Descricao == cirurgia.Descricao)
                        .FirstOrDefault() != null)
                    continue;

                await CirurgiaNegocio.SaveOneAsync(ContextoCirurgias.Collection, cirurgia);
            }
        }
    }
}