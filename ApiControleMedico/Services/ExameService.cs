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

        public IEnumerable<Exame> GetAll()
        {
            var exames = ExameNegocio.GetAll(ContextoExames.Collection);
            return exames;
        }

        public Exame GetOne(string id)
        {
            return ExameNegocio.GetOne(ContextoExames.Collection, id);
        }

        public Exame SaveOne(Exame context)
        {
            ExameNegocio.SaveOne(ContextoExames.Collection, context);

            return context;
        }

        public bool RemoveOne(string id)
        {
            return ExameNegocio.RemoveOne(ContextoExames.Collection, id);
        }

        public void SaveMany(Collection<Exame> exames)
        {
            foreach (var exame in exames)
            {
                if (ContextoExames.Collection.Find(c => c.Descricao == exame.Descricao)
                        .FirstOrDefault() != null)
                    continue;

                ExameNegocio.SaveOne(ContextoExames.Collection, exame);
            }
        }
    }
}