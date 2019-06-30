using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiControleMedico.Modelos;
using ApiControleMedico.Negocio;
using ApiControleMedico.Repositorio;
using ApiControleMedico.Uteis;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace ApiControleMedico.Services
{
    public class MedicoService
    {
        protected readonly DbContexto<Medico> ContextoMedicos;
        protected readonly EntidadeNegocio<Medico> MedicoNegocio = new EntidadeNegocio<Medico>();

        public MedicoService()
        {
            ContextoMedicos = new DbContexto<Medico>("medico");
        }

        public async Task<IEnumerable<Medico>> GetAllAsync()
        {
            return await MedicoNegocio.GetAllAsync(ContextoMedicos.Collection);
        }

        public Task<Medico> GetOneAsync(string id)
        {
            return MedicoNegocio.GetOneAsync(ContextoMedicos.Collection, id);

        }

        public async Task<Medico> SaveOneAsync(Medico medico)
        {
            var usuarioService = new UsuarioService();
            if (medico.ConfiguracaoAgenda.ConfiguracaoAgendaDias.Count > 0)
                medico.ConfiguracaoAgenda = AgendaMedicoNegocio.ConfigurarAgendaMedico(medico.ConfiguracaoAgenda);

            await MedicoNegocio.SaveOneAsync(ContextoMedicos.Collection, medico);

            var usuario = await usuarioService.CriarNovoUsuarioMedico(medico);

            medico.Usuario = usuario;
            await MedicoNegocio.SaveOneAsync(ContextoMedicos.Collection, medico);


            return medico;
        }

        public Task<bool> RemoveOneAsync(string id)
        {
            var usuarioService = new UsuarioService();
            var medico = MedicoNegocio.GetOneAsync(ContextoMedicos.Collection, id).Result;
            usuarioService.RemoveOneAsync(medico.Usuario.Id);

            return MedicoNegocio.RemoveOneAsync(ContextoMedicos.Collection, id);
        }


        public Medico BuscarMedicoUsuario(Usuario usuario)
        {
            return ContextoMedicos.Collection.Find(c => c.Usuario.Id == usuario.Id).FirstOrDefault();
        }

        public async Task<ActionResult<List<Medico>>> TodosFiltrandoMedico(string medicoId)
        {
            var medico = await this.GetOneAsync(medicoId);

            if (medico.Administrador)
                return MedicoNegocio.GetAllAsync(ContextoMedicos.Collection).Result.ToList(); // where clinica == clinica

            return ContextoMedicos.Collection.Find(c => c.Id == medicoId).ToList();

        }
    }


}