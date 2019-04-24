using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ApiControleMedico.Modelos;
using ApiControleMedico.Repositorio;
using MongoDB.Driver;

namespace ApiControleMedico.Services
{
    public class ExameService
    {
        protected readonly DbContexto<Exame> ContextoExames;
        protected readonly EntidadeNegocio<Exame> ExameNegocio = new EntidadeNegocio<Exame>();

        public ExameService()
        {
            ContextoExames = new DbContexto<Exame>("exame");
        }

        public async Task<IEnumerable<Exame>> GetAllAsync()
        {
            var exames = await ExameNegocio.GetAllAsync(ContextoExames.Collection);
            return exames;
        }

        public Task<Exame> GetOneAsync(string id)
        {
            return ExameNegocio.GetOneAsync(ContextoExames.Collection, id);
        }

        public async Task<Exame> SaveOneAsync(Exame context)
        {
            await ExameNegocio.SaveOneAsync(ContextoExames.Collection, context);

            return context;
        }

        public Task<bool> RemoveOneAsync(string id)
        {
            return ExameNegocio.RemoveOneAsync(ContextoExames.Collection, id);
        }

        public async void SaveManyAsync(Collection<Exame> exames)
        {
            foreach (var exame in exames)
            {
                if (ContextoExames.Collection.Find(c => c.Descricao == exame.Descricao)
                        .FirstOrDefault() != null)
                    continue;

                await ExameNegocio.SaveOneAsync(ContextoExames.Collection, exame);
            }
        }
    }
}