using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ApiControleMedico.Modelos;
using ApiControleMedico.Repositorio;
using MongoDB.Driver;

namespace ApiControleMedico.Services
{
    public class EspecialidadeService
    {
        protected readonly DbContexto<Especialidade> ContextoEspecialidades;
        protected readonly EntidadeNegocio<Especialidade> EspecialidadeNegocio = new EntidadeNegocio<Especialidade>();

        public EspecialidadeService()
        {
            ContextoEspecialidades = new DbContexto<Especialidade>("especialidade");
        }

        public async Task<IEnumerable<Especialidade>> GetAllAsync()
        {
            var especialidades = await EspecialidadeNegocio.GetAllAsync(ContextoEspecialidades.Collection);
            return especialidades;
        }

        public Task<Especialidade> GetOneAsync(string id)
        {
            return EspecialidadeNegocio.GetOneAsync(ContextoEspecialidades.Collection, id);
        }

        public async Task<Especialidade> SaveOneAsync(Especialidade context)
        {
            await EspecialidadeNegocio.SaveOneAsync(ContextoEspecialidades.Collection, context);

            return context;
        }

        public Task<bool> RemoveOneAsync(string id)
        {
            return EspecialidadeNegocio.RemoveOneAsync(ContextoEspecialidades.Collection, id);
        }

        public async void SaveManyAsync(Collection<Especialidade> especialidades)
        {
            foreach (var especialidade in especialidades)
            {
                if (ContextoEspecialidades.Collection.Find(c => c.Descricao == especialidade.Descricao)
                        .FirstOrDefault() != null)
                    continue;

                await EspecialidadeNegocio.SaveOneAsync(ContextoEspecialidades.Collection, especialidade);
            }
        }
    }
}