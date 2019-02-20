using System.Collections.Generic;
using System.Threading.Tasks;
using ApiControleMedico.Modelos;
using ApiControleMedico.Repositorio;

namespace ApiControleMedico.Services
{
    public class MedicoService : ILogic<Medico>
    {
        protected readonly DbContexto<Medico> Medicos;
        protected readonly EntidadeNegocio<Medico> MedicoNegocio = new EntidadeNegocio<Medico>();

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

        public Task<Medico> GetOneAsync(string id)
        {
            throw new System.NotImplementedException();
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

        public Task<bool> RemoveOneAsync(string id)
        {
            throw new System.NotImplementedException();
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