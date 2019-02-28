using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiControleMedico.Modelos;
using ApiControleMedico.Repositorio;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ApiControleMedico.Services
{
    public class ConvenioMedicoService : ILogic<ConvenioMedico>
    {
        protected readonly DbContexto<ConvenioMedico> ConvenioMedicos;
        protected readonly EntidadeNegocio<ConvenioMedico> ConvenioMedicoNegocio = new EntidadeNegocio<ConvenioMedico>();

        public ConvenioMedicoService()
        {
            ConvenioMedicos = new DbContexto<ConvenioMedico>("convenioMedico");
        }

        public List<ConvenioMedico> BuscarConvenioMedico(string medicoId)
        {
            return ConvenioMedicos.Collection.Find(c => c.MedicoId == medicoId).ToList();
;
        }

        public async Task<IEnumerable<ConvenioMedico>> GetAllAsync()
        {
            return await ConvenioMedicoNegocio.GetAllAsync(ConvenioMedicos.Collection);
        }

        public Task<ConvenioMedico> GetOneAsync(ConvenioMedico convenioMedico)
        {
            throw new System.NotImplementedException();
        }

        public Task<ConvenioMedico> GetOneAsync(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<ConvenioMedico> GetManyAsync(IEnumerable<ConvenioMedico> convenioMedicos)
        {
            throw new System.NotImplementedException();
        }

        public Task<ConvenioMedico> GetManyAsync(IEnumerable<string> ids)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ConvenioMedico> SaveOneAsync(ConvenioMedico convenioMedico)
        {
            if (string.IsNullOrEmpty(convenioMedico.Id))
            {
                convenioMedico.Id = ObjectId.GenerateNewId().ToString();
            }
            await ConvenioMedicoNegocio.SaveOneAsync(ConvenioMedicos.Collection, convenioMedico);
            return convenioMedico;
        }

        public Task<ConvenioMedico> SaveManyAsync(IEnumerable<ConvenioMedico> convenioMedicos)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> RemoveOneAsync(ConvenioMedico convenioMedico)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> RemoveOneAsync(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> RemoveManyAsync(IEnumerable<ConvenioMedico> convenioMedicos)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> RemoveManyAsync(IEnumerable<string> ids)
        {
            throw new System.NotImplementedException();
        }
    }
}