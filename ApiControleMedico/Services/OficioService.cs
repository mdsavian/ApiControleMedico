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

        public async Task<IEnumerable<Oficio>> GetAllAsync()
        {
            var oficios = await OficioNegocio.GetAllAsync(ContextoOficios.Collection);
            return oficios;
        }

        public Task<Oficio> GetOneAsync(string id)
        {
            return OficioNegocio.GetOneAsync(ContextoOficios.Collection, id);
        }

        public async Task<Oficio> SaveOneAsync(Oficio context)
        {
            await OficioNegocio.SaveOneAsync(ContextoOficios.Collection, context);

            return context;
        }

        public Task<bool> RemoveOneAsync(string id)
        {
            return OficioNegocio.RemoveOneAsync(ContextoOficios.Collection, id);
        }
    }
}