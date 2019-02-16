using System.Collections.Generic;
using System.Threading.Tasks;
using ApiControleMedico.Modelos;
using ApiControleMedico.Repositorio;

namespace ApiControleMedico.Services
{
    public class PacienteService : ILogic<Paciente>
    {
        protected readonly DbContexto<Paciente> Pacientes;
        protected readonly EntidadeNegocio<Paciente, Paciente> PacienteNegocio = new EntidadeNegocio<Paciente, Paciente>();

        public PacienteService()
        {
            Pacientes = new DbContexto<Paciente>("paciente");
        }

        public async Task<IEnumerable<Paciente>> GetAllAsync()
        {
            var pacientes = await PacienteNegocio.GetAllAsync(Pacientes.Collection);
            return pacientes;
        }

        public Task<Paciente> GetOneAsync(Paciente context)
        {
            throw new System.NotImplementedException();
        }

        public Task<Paciente> GetOneAsync(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<Paciente> GetManyAsync(IEnumerable<Paciente> contexts)
        {
            throw new System.NotImplementedException();
        }

        public Task<Paciente> GetManyAsync(IEnumerable<string> ids)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Paciente> SaveOneAsync(Paciente context)
        {
            await PacienteNegocio.SaveOneAsync(Pacientes.Collection, context);

            return context;
        }

        public Task<Paciente> SaveManyAsync(IEnumerable<Paciente> contexts)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> RemoveOneAsync(Paciente context)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> RemoveOneAsync(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> RemoveManyAsync(IEnumerable<Paciente> contexts)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> RemoveManyAsync(IEnumerable<string> ids)
        {
            throw new System.NotImplementedException();
        }
    }
}