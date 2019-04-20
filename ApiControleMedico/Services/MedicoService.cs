using System.Collections.Generic;
using System.Threading.Tasks;
using ApiControleMedico.Modelos;
using ApiControleMedico.Negocio;
using ApiControleMedico.Repositorio;
using ApiControleMedico.Uteis;
using MongoDB.Driver;

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

        public async Task<Medico> SalvarConfiguracaoAgenda(Medico medico)
        {

            var usuarioService = new UsuarioService();

            await MedicoNegocio.SaveOneAsync(Medicos.Collection, medico);

            var usuario = await usuarioService.CriarNovoUsuarioMedico(medico);

            medico.Usuario = usuario;
            await MedicoNegocio.SaveOneAsync(Medicos.Collection, medico);


            return medico;
        }

        public async Task<Medico> SaveOneAsync(Medico medico)
        {
            var usuarioService = new UsuarioService();
            medico.ConfiguracaoAgenda = AgendaMedicoNegocio.ConfigurarAgendaMedico(medico.ConfiguracaoAgenda);

            await MedicoNegocio.SaveOneAsync(Medicos.Collection, medico);

            var usuario = await usuarioService.CriarNovoUsuarioMedico(medico);

            medico.Usuario = usuario;
            await MedicoNegocio.SaveOneAsync(Medicos.Collection, medico);


            return medico;
        }
        
        public Task<bool> RemoveOneAsync(string id)
        {
            return MedicoNegocio.RemoveOneAsync(Medicos.Collection, id);
        }


        public Medico BuscarMedicoUsuario(Usuario usuario)
        {
            return Medicos.Collection.Find(c => c.Usuario.Id == usuario.Id).FirstOrDefault();
        }
    }

   
}