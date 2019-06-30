using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiControleMedico.Modelos;
using ApiControleMedico.Repositorio;
using MongoDB.Driver;

namespace ApiControleMedico.Services
{
    public class ConvenioService
    {
        protected readonly DbContexto<Convenio> ContextoConvenio;
        protected readonly EntidadeNegocio<Convenio> ConvenioNegocio = new EntidadeNegocio<Convenio>();

        public ConvenioService()
        {
            ContextoConvenio = new DbContexto<Convenio>("convenio");
        }

        public async Task<IEnumerable<Convenio>> GetAllAsync()
        {
            var convenios = await ConvenioNegocio.GetAllAsync(ContextoConvenio.Collection);
            return convenios;
        }

        public Task<Convenio> GetOneAsync(string id)
        {
            return ConvenioNegocio.GetOneAsync(ContextoConvenio.Collection, id);
        }

        public async Task<Convenio> SaveOneAsync(Convenio convenio)
        {
            await ConvenioNegocio.SaveOneAsync(ContextoConvenio.Collection, convenio);
            return convenio;
        }


        public Task<bool> RemoveOneAsync(string id)
        {
            return ConvenioNegocio.RemoveOneAsync(ContextoConvenio.Collection, id);
        }

        public List<Medico> BuscarMedicosPorConvenio(string convenioId)
        {
            var medicos = new MedicoService().GetAllAsync().Result;

            var xx = medicos.Where(c => c.Convenios.Any(d => d.Id == convenioId)).ToList();

            return xx;
        }

        public List<Convenio> TodosFiltrandoMedico(string medicoId)
        {
            var conveniosMedicos = new MedicoService().GetOneAsync(medicoId).Result.Convenios;
            try
            {
                var filter = Builders<Convenio>.Filter.Nin(c => c.Id, conveniosMedicos.Select(c => c.Id));
                var convenios = ContextoConvenio.Collection.Find(filter).ToList();
                return convenios;
            }
            catch (Exception ex)
            {

            }

            return ContextoConvenio.Collection.Find(c => true).ToList();

        }
    }
}