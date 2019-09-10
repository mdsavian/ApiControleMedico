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

        public IEnumerable<Medico> GetAll()
        {
            return MedicoNegocio.GetAll(ContextoMedicos.Collection);
        }

        public Medico GetOne(string id)
        {
            return MedicoNegocio.GetOne(ContextoMedicos.Collection, id);
        }

        public Medico SalvarConfiguracaoMedico(Medico medico)
        {

            if (medico.ConfiguracaoAgenda?.ConfiguracaoAgendaDias.Count > 0)
            {
                var configuracaoAgenda = AgendaMedicoNegocio.ConfigurarAgendaMedico(medico.ConfiguracaoAgenda);
                medico.ConfiguracaoAgendaId = configuracaoAgenda.Id;
                medico.ConfiguracaoAgenda = configuracaoAgenda;
            }

            MedicoNegocio.SaveOne(ContextoMedicos.Collection, medico);


            return medico;
        }

        public Medico SaveOne(Medico medico)
        {

            MedicoNegocio.SaveOne(ContextoMedicos.Collection, medico);

            if (medico.UsuarioId.IsNullOrWhiteSpace())
            {
                var usuarioService = new UsuarioService();

                var usuario = usuarioService.CriarNovoUsuarioMedico(medico);

                medico.UsuarioId = usuario.Id;
                MedicoNegocio.SaveOne(ContextoMedicos.Collection, medico);
            }


            return medico;
        }

        public bool RemoveOne(string id)
        {
            try
            {
                var usuarioService = new UsuarioService();
                var medico = MedicoNegocio.GetOne(ContextoMedicos.Collection, id);
                if (!medico.UsuarioId.IsNullOrWhiteSpace())
                    usuarioService.RemoveOne(medico.UsuarioId);

                return MedicoNegocio.RemoveOne(ContextoMedicos.Collection, id);
            }
            catch (Exception ex)
            {

            }

            return false;
        }

        internal List<Medico> BuscarMedicoConvenio(string convenioId)
        {
            return ContextoMedicos.Collection.AsQueryable().Where(c => c.ConveniosId.Contains(convenioId)).ToList();
        }

        internal List<Medico> BuscarMedicoEspecialidade(string especialidadeId)
        {
            return ContextoMedicos.Collection.AsQueryable().Where(c => c.EspecialidadeId == especialidadeId).ToList();
        }

        public Medico BuscarMedicoUsuario(Usuario usuario)
        {
            return ContextoMedicos.Collection.Find(c => c.UsuarioId == usuario.Id).FirstOrDefault();
        }

        public ActionResult<List<Medico>> TodosFiltrandoMedico(string medicoId)
        {
            var medico = this.GetOne(medicoId);

            if (medico.Administrador)
                return MedicoNegocio.GetAll(ContextoMedicos.Collection).ToList(); // where clinica == clinica

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