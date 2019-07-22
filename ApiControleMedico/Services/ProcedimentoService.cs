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

        public IEnumerable<Procedimento> GetAll()
        {
            var procedimentos = ProcedimentoNegocio.GetAll(ContextoProcedimentos.Collection);
            return procedimentos;
        }

        public Procedimento GetOne(string id)
        {
            return ProcedimentoNegocio.GetOne(ContextoProcedimentos.Collection, id);
        }

        public Procedimento SaveOne(Procedimento context)
        {
            ProcedimentoNegocio.SaveOne(ContextoProcedimentos.Collection, context);

            return context;
        }

        public bool RemoveOne(string id)
        {
            return ProcedimentoNegocio.RemoveOne(ContextoProcedimentos.Collection, id);
        }

        public void SaveMany(Collection<Procedimento> procedimentos)
        {
            foreach (var procedimento in procedimentos)
            {
                if (ContextoProcedimentos.Collection.Find(c => c.Descricao == procedimento.Descricao)
                        .FirstOrDefault() != null)
                    continue;

                ProcedimentoNegocio.SaveOne(ContextoProcedimentos.Collection, procedimento);
            }
        }
    }
}