using System.Collections.Generic;
using System.Threading.Tasks;
using ApiControleMedico.Modelos;
using ApiControleMedico.Repositorio;

namespace ApiControleMedico.Services
{
    public class OficioService
    {
        protected readonly DbContexto<Oficio> ContextoOficios;
        protected readonly EntidadeNegocio<Oficio> OficioNegocio = new EntidadeNegocio<Oficio>();

        public OficioService()
        {
            ContextoOficios = new DbContexto<Oficio>("oficio");
        }

        public IEnumerable<Oficio> GetAll()
        {
            var oficios = OficioNegocio.GetAll(ContextoOficios.Collection);
            return oficios;
        }

        public Oficio GetOne(string id)
        {
            return OficioNegocio.GetOne(ContextoOficios.Collection, id);
        }

        public Oficio SaveOne(Oficio context)
        {
            OficioNegocio.SaveOne(ContextoOficios.Collection, context);

            return context;
        }

        public bool RemoveOne(string id)
        {
            return OficioNegocio.RemoveOne(ContextoOficios.Collection, id);
        }
    }
}