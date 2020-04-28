using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ApiControleMedico.Modelos;
using ApiControleMedico.Negocio;
using ApiControleMedico.Repositorio;
using ApiControleMedico.Uteis;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace ApiControleMedico.Services
{
    public class MedicoService
    {
        protected readonly DbContexto<Medico> ContextoMedicos;
        protected readonly DbContexto<Agendamento> ContextoAgendamentos;
        protected readonly EntidadeNegocio<Medico> MedicoNegocio = new EntidadeNegocio<Medico>();

        public MedicoService()
        {
            ContextoMedicos = new DbContexto<Medico>("medico");
            ContextoAgendamentos = new DbContexto<Agendamento>("agendamento");

        }

        public IEnumerable<Medico> GetAll()
        {
            var medicos = MedicoNegocio.GetAll(ContextoMedicos.Collection).OrderBy(c => c.NomeCompleto).ToList();
            foreach (var func in medicos)
            {
                func.FotoId = string.Empty;
                this.SaveOne(func);
            }

            return medicos;
        }

        public Medico GetOne(string id)
        {
            var medico = MedicoNegocio.GetOne(ContextoMedicos.Collection, id);
            medico.Especialidade = this.CarregarEspecialidadeMedico(medico);

            return medico;
        }

        //public Medico SalvarConfiguracaoMedico(Medico medico)
        //{
        //    if (medico.ConfiguracaoAgenda?.ConfiguracaoAgendaDias.Count > 0)
        //    {
        //        var configuracaoAgenda = ConfiguracaoAgendaService.ConfigurarAgendaMedico(medico.ConfiguracaoAgenda);
        //        medico.ConfiguracaoAgendaId = configuracaoAgenda.Id;
        //        medico.ConfiguracaoAgenda = configuracaoAgenda;
        //    }

        //    MedicoNegocio.SaveOne(ContextoMedicos.Collection, medico);


        //    return medico;
        //}

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

        internal Especialidade CarregarEspecialidadeMedico(Medico medico)
        {
            var especialidadeService = new EspecialidadeService();

            if (!medico.EspecialidadeId.IsNullOrWhiteSpace())
                return especialidadeService.GetOne(medico.EspecialidadeId);

            return null;
        }

        public List<Medico> Todos(bool carregarEspecialidade)
        {
            var medicos = ContextoMedicos.Collection.AsQueryable().ToList();

            if (carregarEspecialidade)
            {
                foreach (var medico in medicos)
                    medico.Especialidade = this.CarregarEspecialidadeMedico(medico);
            }

            return medicos;
        }

        internal List<Medico> BuscarMedicosPorUsuario(string usuarioId, string clinicaId, bool carregarEspecialidade)
        {
            var listaMedicos = new List<Medico>();
            var usuario = new UsuarioService().GetOne(usuarioId);

            if (!usuario.FuncionarioId.IsNullOrWhiteSpace())
            {
                var funcionario = new FuncionarioService().GetOne(usuario.FuncionarioId);
                if (funcionario.MedicosId.HasItems())
                {
                    var medicos = ContextoMedicos.Collection.AsQueryable().Where(c => funcionario.MedicosId.Contains(c.Id) && c.ClinicasId.Contains(clinicaId)).ToList();

                    if (carregarEspecialidade)
                    {
                        foreach (var medico in medicos)
                            medico.Especialidade = this.CarregarEspecialidadeMedico(medico);
                    }

                    return medicos;
                }

            }
            else if (!usuario.MedicoId.IsNullOrWhiteSpace())
            {
                var medicoUsuario = ContextoMedicos.Collection.Find(c => c.Id == usuario.MedicoId && c.ClinicasId.Contains(clinicaId)).FirstOrDefault();

                if (medicoUsuario != null)
                {
                    if (medicoUsuario.Administrador)
                    {
                        var medicos = ContextoMedicos.Collection.AsQueryable().Where(c => c.ClinicasId.Contains(clinicaId)).ToList();

                        if (carregarEspecialidade)
                        {
                            foreach (var medicEspecialidade in medicos)
                                medicEspecialidade.Especialidade = this.CarregarEspecialidadeMedico(medicEspecialidade);
                        }

                        return medicos;
                    }
                    else
                    {
                        if (carregarEspecialidade)
                            medicoUsuario.Especialidade = this.CarregarEspecialidadeMedico(medicoUsuario);

                        listaMedicos.Add(medicoUsuario);
                    }
                }
            }

            return listaMedicos;
        }

        internal ActionResult<bool> ValidarDeleteConvenioMedico(string medicoId, string convenioId)
        {
            return ContextoAgendamentos.Collection.AsQueryable().FirstOrDefault(c => c.MedicoId == medicoId && c.ConvenioId == convenioId) != null;
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

        public ActionResult<List<Medico>> TodosFiltrandoMedico(string medicoId)
        {
            var medico = this.GetOne(medicoId);

            if (medico.Administrador)
                return MedicoNegocio.GetAll(ContextoMedicos.Collection).ToList(); // where clinica == clinica

            return ContextoMedicos.Collection.Find(c => c.Id == medicoId).ToList();

        }

        public ConfiguracaoAgenda BuscarConfiguracaoAgenda(string medicoId, string clinicaId)
        {
            using (var contexto = new DbContexto<ConfiguracaoAgenda>("configuracaoAgenda"))
            {
                return contexto.Collection.Find(c => c.MedicoId == medicoId && c.ClinicaId == clinicaId).FirstOrDefault();
            }
        }

        public void SalvarFoto(string medicoId, string nomeArquivo, string caminhoArquivo)
        {
            var gridFs = new GridFSBucket(ContextoMedicos.Database);
            string idFoto = "";

            var medico = ContextoMedicos.Collection.Find(c => c.Id == medicoId).FirstOrDefault();
            if (medico != null)
            {
                if (!string.IsNullOrEmpty(medico.FotoId))
                    gridFs.DeleteAsync(new ObjectId(medico.FotoId));

                using (var foto = File.OpenRead(caminhoArquivo))
                {
                    var task = Task.Run(() =>
                    {
                        return gridFs.UploadFromStreamAsync(nomeArquivo, foto);
                    });
                    idFoto = task.Result.ToString();
                }

                medico.FotoId = idFoto;
                MedicoNegocio.SaveOne(ContextoMedicos.Collection, medico);

            }
        }

        public byte[] DownloadFoto(string medicoId)
        {
            var gridFs = new GridFSBucket(ContextoMedicos.Database);
            try
            {
                var task = gridFs.DownloadAsBytesByNameAsync($"medico-{medicoId}.jpeg");
                Task.WaitAll(task);
                var bytes = task.Result;
                return bytes;
            }
            catch (Exception ex)
            {

            }

            return null;
        }
    }


}