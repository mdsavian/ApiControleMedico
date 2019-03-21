using System.Collections.Generic;
using System.Threading.Tasks;
using ApiControleMedico.Modelos;
using ApiControleMedico.Repositorio;
using ApiControleMedico.Uteis;

namespace ApiControleMedico.Services
{
    public class MedicoService
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

        public Task<Medico> GetOneAsync(string id)
        {
            return MedicoNegocio.GetOneAsync(Medicos.Collection, id);

        }

        public async Task<Medico> SaveOneAsync(Medico medico)
        {
            if (medico.Id.IsNullOrWhiteSpace())
                new UsuarioService().CriarNovoUsuarioMedico(medico);

            await MedicoNegocio.SaveOneAsync(Medicos.Collection, medico);
            
            return medico;
        }
        
        public Task<bool> RemoveOneAsync(string id)
        {
            return MedicoNegocio.RemoveOneAsync(Medicos.Collection, id);
        }


    }
}