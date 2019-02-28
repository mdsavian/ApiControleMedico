using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiControleMedico.Modelos;
using ApiControleMedico.Repositorio;
using MongoDB.Bson;

namespace ApiControleMedico.Services
{
    public class ConvenioService : ILogic<Convenio>
    {
        protected readonly DbContexto<Convenio> Convenios;
        protected readonly EntidadeNegocio<Convenio> ConvenioNegocio = new EntidadeNegocio<Convenio>();
        protected readonly EntidadeNegocio<ConvenioMedico> ConvenioMedicoNegocio = new EntidadeNegocio<ConvenioMedico>();

        public ConvenioService()
        {
            Convenios = new DbContexto<Convenio>("convenio");
        }

        public async Task<IEnumerable<Convenio>> GetAllAsync()
        {
            var convenios = await ConvenioNegocio.GetAllAsync(Convenios.Collection);
            return convenios;
        }

        public Task<Convenio> GetOneAsync(Convenio convenio)
        {
            throw new System.NotImplementedException();
        }

        public Task<Convenio> GetOneAsync(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<Convenio> GetManyAsync(IEnumerable<Convenio> convenios)
        {
            throw new System.NotImplementedException();
        }

        public Task<Convenio> GetManyAsync(IEnumerable<string> ids)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Convenio> SaveOneAsync(Convenio convenio)
        {
            await ConvenioNegocio.SaveOneAsync(Convenios.Collection, convenio);
            return convenio;
        }

        public Task<Convenio> SaveManyAsync(IEnumerable<Convenio> convenios)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> RemoveOneAsync(Convenio convenio)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> RemoveOneAsync(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> RemoveManyAsync(IEnumerable<Convenio> convenios)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> RemoveManyAsync(IEnumerable<string> ids)
        {
            throw new System.NotImplementedException();
        }
    }
}