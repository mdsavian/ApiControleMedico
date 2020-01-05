using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ApiControleMedico.Modelos;
using ApiControleMedico.Repositorio;
using MongoDB.Driver;

namespace ApiControleMedico.Services
{
    public class ModeloPrescricaoService
    {
        protected readonly DbContexto<ModeloPrescricao> ContextoModeloPrescricaos;
        protected readonly EntidadeNegocio<ModeloPrescricao> ModeloPrescricaoNegocio = new EntidadeNegocio<ModeloPrescricao>();

        public ModeloPrescricaoService()
        {
            ContextoModeloPrescricaos = new DbContexto<ModeloPrescricao>("modeloPrescricao");
        }

        public IEnumerable<ModeloPrescricao> GetAll()
        {
            var modeloPrescricaos = ModeloPrescricaoNegocio.GetAll(ContextoModeloPrescricaos.Collection);
            return modeloPrescricaos;
        }

        public ModeloPrescricao GetOne(string id)
        {
            return ModeloPrescricaoNegocio.GetOne(ContextoModeloPrescricaos.Collection, id);
        }

        public ModeloPrescricao SaveOne(ModeloPrescricao context)
        {
            ModeloPrescricaoNegocio.SaveOne(ContextoModeloPrescricaos.Collection, context);

            return context;
        }

        public bool RemoveOne(string id)
        {
            return ModeloPrescricaoNegocio.RemoveOne(ContextoModeloPrescricaos.Collection, id);
        }

        public void SaveMany(Collection<ModeloPrescricao> modeloPrescricaos)
        {
            foreach (var modeloPrescricao in modeloPrescricaos)
            {
                if (ContextoModeloPrescricaos.Collection.Find(c => c.Descricao == modeloPrescricao.Descricao)
                        .FirstOrDefault() != null)
                    continue;

                ModeloPrescricaoNegocio.SaveOne(ContextoModeloPrescricaos.Collection, modeloPrescricao);
            }
        }
    }
}