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

        public IEnumerable<Local> GetAll()
        {
            var locals = LocalNegocio.GetAll(ContextoLocals.Collection);
            return locals;
        }

        public Local GetOne(string id)
        {
            return LocalNegocio.GetOne(ContextoLocals.Collection, id);
        }

        public Local SaveOne(Local context)
        {
            LocalNegocio.SaveOne(ContextoLocals.Collection, context);

            return context;
        }

        public bool RemoveOne(string id)
        {
            return LocalNegocio.RemoveOne(ContextoLocals.Collection, id);
        }

        public void SaveMany(Collection<Local> locals)
        {
            foreach (var local in locals)
            {
                if (ContextoLocals.Collection.Find(c => c.Descricao == local.Descricao)
                        .FirstOrDefault() != null)
                    continue;

                LocalNegocio.SaveOne(ContextoLocals.Collection, local);
            }
        }
    }
}