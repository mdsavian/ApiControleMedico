using System;
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

        public async Task<Medico> SalvarConfiguracaoMedico(Medico medico)
        {

            if (medico.ConfiguracaoAgenda?.ConfiguracaoAgendaDias.Count > 0)
            {
                var configuracaoAgenda = await AgendaMedicoNegocio.ConfigurarAgendaMedico(medico.ConfiguracaoAgenda);
                medico.ConfiguracaoAgendaId = configuracaoAgenda.Id;
                medico.ConfiguracaoAgenda = configuracaoAgenda;
            }

            await MedicoNegocio.SaveOneAsync(ContextoMedicos.Collection, medico);


            return medico;
        }

        public async Task<Medico> SaveOneAsync(Medico medico)
        { 

            await MedicoNegocio.SaveOneAsync(ContextoMedicos.Collection, medico);

            if (medico.UsuarioId.IsNullOrWhiteSpace())
            {
                var usuarioService = new UsuarioService();

                var usuario = await usuarioService.CriarNovoUsuarioMedico(medico);

                medico.UsuarioId = usuario.Id;
                await MedicoNegocio.SaveOneAsync(ContextoMedicos.Collection, medico);
            }


            return medico;
        }

        public Task<bool> RemoveOneAsync(string id)
        {
            try
            {
                var usuarioService = new UsuarioService();
                var medico = MedicoNegocio.GetOneAsync(ContextoMedicos.Collection, id).Result;
                if (!medico.UsuarioId.IsNullOrWhiteSpace())
                    usuarioService.RemoveOneAsync(medico.UsuarioId);

                return MedicoNegocio.RemoveOneAsync(ContextoMedicos.Collection, id);
            }
            catch (Exception ex)
            {

            }

            return null;
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

        public ConfiguracaoAgenda BuscarConfiguracaoAgenda(string configuracaoAgendaId)
        {
            using (var contexto = new DbContexto<ConfiguracaoAgenda>("configuracaoAgenda"))
            {
                return contexto.Collection.Find(c => c.Id == configuracaoAgendaId).FirstOrDefault();
            }
        }
    }


}