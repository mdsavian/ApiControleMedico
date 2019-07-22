using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public IEnumerable<Cirurgia> GetAll()
        {
            var cirurgias = CirurgiaNegocio.GetAll(ContextoCirurgias.Collection);
            return cirurgias;
        }

        public Cirurgia GetOne(string id)
        {
            return CirurgiaNegocio.GetOne(ContextoCirurgias.Collection, id);
        }

        public Cirurgia SaveOne(Cirurgia context)
        {
            CirurgiaNegocio.SaveOne(ContextoCirurgias.Collection, context);

            return context;
        }

        public bool RemoveOne(string id)
        {
            return CirurgiaNegocio.RemoveOne(ContextoCirurgias.Collection, id);
        }

        public void SaveMany(Collection<Cirurgia> cirurgias)
        {
            foreach (var cirurgia in cirurgias)
            {
                if (ContextoCirurgias.Collection.Find(c => c.Descricao == cirurgia.Descricao)
                        .FirstOrDefault() != null)
                    continue;

                CirurgiaNegocio.SaveOne(ContextoCirurgias.Collection, cirurgia);
            }
        }
    }
}