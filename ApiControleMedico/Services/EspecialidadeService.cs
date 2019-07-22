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

        public IEnumerable<Especialidade> GetAll()
        {
            var especialidades = EspecialidadeNegocio.GetAll(ContextoEspecialidades.Collection);
            return especialidades;
        }

        public Especialidade GetOne(string id)
        {
            return EspecialidadeNegocio.GetOne(ContextoEspecialidades.Collection, id);
        }

        public Especialidade SaveOne(Especialidade context)
        {
            EspecialidadeNegocio.SaveOne(ContextoEspecialidades.Collection, context);

            return context;
        }

        public bool RemoveOne(string id)
        {
            return EspecialidadeNegocio.RemoveOne(ContextoEspecialidades.Collection, id);
        }

        public void SaveMany(Collection<Especialidade> especialidades)
        {
            foreach (var especialidade in especialidades)
            {
                if (ContextoEspecialidades.Collection.Find(c => c.Descricao == especialidade.Descricao)
                        .FirstOrDefault() != null)
                    continue;

                EspecialidadeNegocio.SaveOne(ContextoEspecialidades.Collection, especialidade);
            }
        }
    }
}