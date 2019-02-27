using System.Collections.Generic;
using System.Threading.Tasks;
using ApiControleMedico.Modelos;
using ApiControleMedico.Repositorio;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace ApiControleMedico.Services
{
    public class MedicoService 
    {
        protected readonly DbContexto<Medico> Medicos;
        protected readonly EntidadeNegocio<Medico> MedicoNegocio = new EntidadeNegocio<Medico>();
        protected readonly EntidadeNegocio<Convenio> ConvenioNegocio = new EntidadeNegocio<Convenio>();

        public MedicoService()
        {
            Medicos = new DbContexto<Medico>("medico");
        }

        public async Task<IEnumerable<Medico>> GetAllAsync()
        {
            var pacientes = await MedicoNegocio.GetAllAsync(Medicos.Collection);
            return pacientes;
        }

        public Task<Medico> GetOneAsync(Medico medico)
        {
            throw new System.NotImplementedException();
        }

        public Medico GetOneAsync(string id)
        {
            return Medicos.Collection.Find(c => c.Id == id).First();

        }

        public Task<Medico> GetManyAsync(IEnumerable<Medico> medicos)
        {
            throw new System.NotImplementedException();
        }

        public Task<Medico> GetManyAsync(IEnumerable<string> ids)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Medico> SaveOneAsync(Medico medico)
        {
            if (string.IsNullOrEmpty(medico.Id))
            {
                medico.Id = ObjectId.GenerateNewId().ToString();

                new UsuarioService().CriarNovoUsuarioMedico(medico);
            }
            await MedicoNegocio.SaveOneAsync(Medicos.Collection, medico);
            return medico;
        }

        public Task<Medico> SaveManyAsync(IEnumerable<Medico> medicos)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> RemoveOneAsync(Medico medico)
        {
            throw new System.NotImplementedException();
        }
        
        public bool RemoveOneAsync(string id)
        {
            return Medicos.Collection.DeleteOne(c => c.Id == id).DeletedCount == 1;
        }

        public Task<bool> RemoveManyAsync(IEnumerable<Medico> medicos)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> RemoveManyAsync(IEnumerable<string> ids)
        {
            throw new System.NotImplementedException();
        }
    }
}