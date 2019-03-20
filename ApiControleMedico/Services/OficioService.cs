using System.Collections.Generic;
using System.Threading.Tasks;
using ApiControleMedico.Modelos;
using ApiControleMedico.Repositorio;

namespace ApiControleMedico.Services
{
    public class OficioService
    {
        protected readonly DbContexto<Oficio> Oficios;
        protected readonly EntidadeNegocio<Oficio> OficioNegocio = new EntidadeNegocio<Oficio>();

        public OficioService()
        {
            Oficios = new DbContexto<Oficio>("oficio");
        }

        public async Task<IEnumerable<Oficio>> GetAllAsync()
        {
            var oficios = await OficioNegocio.GetAllAsync(Oficios.Collection);
            return oficios;
        }

        public Task<Oficio> GetOneAsync(string id)
        {
            return OficioNegocio.GetOneAsync(Oficios.Collection, id);
        }

        public async Task<Oficio> SaveOneAsync(Oficio context)
        {
            await OficioNegocio.SaveOneAsync(Oficios.Collection, context);

            return context;
        }

        public Task<bool> RemoveOneAsync(string id)
        {
            return OficioNegocio.RemoveOneAsync(Oficios.Collection, id);
        }
    }
}