using System;
using System.Collections.Generic;
using System.Linq;
using ApiControleMedico.Modelos;
using ApiControleMedico.Modelos.Enums;
using ApiControleMedico.Repositorio;
using ApiControleMedico.Uteis;
using MongoDB.Driver;

namespace ApiControleMedico.Services
{
    public class AgendamentoService
    {
        protected readonly DbContexto<Agendamento> ContextoAgendamentos;
        protected readonly EntidadeNegocio<Agendamento> AgendamentoNegocio = new EntidadeNegocio<Agendamento>();

        public AgendamentoService()
        {
            ContextoAgendamentos = new DbContexto<Agendamento>("agendamento");
        }

        public IEnumerable<Agendamento> GetAll()
        {
            var agendamentos = AgendamentoNegocio.GetAll(ContextoAgendamentos.Collection);
            foreach (var agendamento in agendamentos)
            {
                agendamento.TipoAgendamentoDescricao = this.RetornarDescricaoAgendamento(agendamento);
                agendamento.Paciente = RetornarPaciente(agendamento.PacienteId);
                agendamento.Medico = RetornarMedico(agendamento.MedicoId);
                agendamento.Clinica = RetornarClinica(agendamento.ClinicaId);
            }
            return agendamentos;
        }

        public Agendamento GetOne(string id)
        {
            var agendamento = AgendamentoNegocio.GetOne(ContextoAgendamentos.Collection, id);

            agendamento.TipoAgendamentoDescricao = this.RetornarDescricaoAgendamento(agendamento);
            agendamento.Paciente = RetornarPaciente(agendamento.PacienteId);
            agendamento.Medico = RetornarMedico(agendamento.MedicoId);
            agendamento.Clinica = RetornarClinica(agendamento.ClinicaId);
            return agendamento;
        }

        public Agendamento SaveOne(Agendamento agendamento)
        {
            var offset = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow);
            agendamento.DataAgendamento = new DateTimeOffset(agendamento.DataAgendamento).ToOffset(offset).DateTime;

            TimeSpan ts = new TimeSpan(0, 0, 0);

            agendamento.DataAgendamento = agendamento.DataAgendamento.Date + ts;
            agendamento.TipoAgendamentoDescricao = RetornarDescricaoAgendamento(agendamento);
            agendamento.Paciente = RetornarPaciente(agendamento.PacienteId);
            agendamento.Medico = RetornarMedico(agendamento.MedicoId);
            agendamento.Clinica = RetornarClinica(agendamento.ClinicaId);
            AgendamentoNegocio.SaveOne(ContextoAgendamentos.Collection, agendamento);

            return agendamento;
        }

        public bool RemoveOne(string id)
        {
            return AgendamentoNegocio.RemoveOne(ContextoAgendamentos.Collection, id);
        }

        public List<Agendamento> BuscarAgendamentosMedico(string medicoId, string data, string tipoCalendario)
        {
            try
            {
                var inicioSemana = data.ToDateTime();
                var fimSemana = inicioSemana.AddHours(23).AddMinutes(59).AddSeconds(59);

                if (tipoCalendario == "week")
                {
                    inicioSemana = data.ToDateTime().InicioDaSemana();
                    fimSemana = inicioSemana.AddDays(6).AddHours(23).AddMinutes(59).AddSeconds(59);
                }
                var medicosId = medicoId.Split(",");

                var agendamentos = ContextoAgendamentos.Collection.AsQueryable().Where(c =>
                    medicosId.Contains(c.MedicoId) && c.DataAgendamento >= inicioSemana &&
                    c.DataAgendamento <= fimSemana).ToList();

                foreach (var agendamento in agendamentos)
                {
                    agendamento.TipoAgendamentoDescricao = this.RetornarDescricaoAgendamento(agendamento);
                    agendamento.Paciente = RetornarPaciente(agendamento.PacienteId);
                    agendamento.Medico = RetornarMedico(agendamento.MedicoId);
                    agendamento.Clinica = RetornarClinica(agendamento.ClinicaId);
                }
                return agendamentos;
            }
            catch (Exception ex)
            {
                return new List<Agendamento>();
            }
        }

        public List<Agendamento> BuscarAgendamentosCaixa(string caixaId, string clinicaId)
        {
            var agendamentos = ContextoAgendamentos.Collection.Find(c => c.ClinicaId == clinicaId && c.Pagamentos.Any(d => d.CaixaId == caixaId)).ToList().OrderByDescending(c => c.DataAgendamento).ThenByDescending(c => c.HoraInicial).ToList();

            foreach (var agendamento in agendamentos)
            {
                agendamento.Paciente = RetornarPaciente(agendamento.PacienteId);
                agendamento.Medico = RetornarMedico(agendamento.MedicoId);
                agendamento.Clinica = RetornarClinica(agendamento.ClinicaId);
                agendamento.Convenio = RetornarConvenio(agendamento.ConvenioId);

                agendamento.TipoAgendamentoDescricao = this.RetornarDescricaoAgendamento(agendamento);

            }
            return agendamentos;
        }

        public string RetornarDescricaoAgendamento(Agendamento agendamento)
        {

            if (!string.IsNullOrWhiteSpace(agendamento.ExameId))
                return new ExameService().GetOne(agendamento.ExameId)?.Descricao;

            else if (!string.IsNullOrWhiteSpace(agendamento.CirurgiaId))
                return new CirurgiaService().GetOne(agendamento.CirurgiaId)?.Descricao;

            else if (!string.IsNullOrWhiteSpace(agendamento.ProcedimentoId))
                return new ProcedimentoService().GetOne(agendamento.ProcedimentoId)?.Descricao;
            else
                return agendamento.TipoAgendamento.ToString();
        }

        internal Agendamento BuscarUltimoAgendamentoPaciente(string pacienteId, string agendamentoId)
        {
            var dataHoje = DateTime.Now;
            var agendamento = ContextoAgendamentos.Collection.AsQueryable().OrderByDescending(c => c.DataAgendamento).FirstOrDefault(c => c.PacienteId == pacienteId && (agendamentoId == "" || agendamentoId == null || c.Id != agendamentoId)
             && c.DataAgendamento <= dataHoje);
            if (agendamento != null)
            {
                agendamento.Convenio = RetornarConvenio(agendamento.ConvenioId);
            }

            return agendamento;
        }

        internal List<Agendamento> TodosPorPeriodo(DateTime primeiroDiaMes, DateTime dataHoje, string medicoId, string caixaId, string funcionarioId)
        {
            var agendamentos = ContextoAgendamentos.Collection.Find(c => c.DataAgendamento >= primeiroDiaMes && c.DataAgendamento <= dataHoje && (medicoId.IsNullOrWhiteSpace() || c.MedicoId == medicoId)
                                                             && (caixaId.IsNullOrWhiteSpace() || c.Pagamentos.Any(d => d.CaixaId == caixaId))
                                                             && (funcionarioId.IsNullOrWhiteSpace() || c.FuncionarioId == funcionarioId)).ToList();

            foreach (var agendamento in agendamentos)
            {
                agendamento.Paciente = RetornarPaciente(agendamento.PacienteId);
                agendamento.Medico = RetornarMedico(agendamento.MedicoId);
                agendamento.Clinica = RetornarClinica(agendamento.ClinicaId);
                agendamento.TipoAgendamentoDescricao = this.RetornarDescricaoAgendamento(agendamento);
            }

            return agendamentos;
        }

        internal List<Agendamento> BuscarAgendamentosFuncionario(string funcionarioId)
        {
            return ContextoAgendamentos.Collection.Find(c => c.FuncionarioId == funcionarioId).ToList();
        }

        internal List<Agendamento> BuscarAgendamentosCirurgia(string cirurgiaId)
        {
            return ContextoAgendamentos.Collection.Find(c => c.CirurgiaId == cirurgiaId).ToList();
        }

        internal List<Agendamento> BuscarAgendamentosExame(string exameId)
        {
            return ContextoAgendamentos.Collection.Find(c => c.ExameId == exameId).ToList();
        }

        internal List<Agendamento> BuscarAgendamentosConvenio(string convenioId)
        {
            return ContextoAgendamentos.Collection.Find(c => c.ConvenioId == convenioId).ToList();
        }

        internal List<Agendamento> BuscarAgendamentosProcedimento(string procedimentoId)
        {
            return ContextoAgendamentos.Collection.Find(c => c.ProcedimentoId == procedimentoId).ToList();
        }

        internal List<Agendamento> BuscarAgendamentosLocal(string localId)
        {
            return ContextoAgendamentos.Collection.Find(c => c.LocalId == localId).ToList();
        }

        internal List<Agendamento> BuscarAgendamentosPaciente(string pacienteId, string usuarioId, string clinicaId)
        {
            var medicos = new MedicoService().BuscarMedicosPorUsuario(usuarioId, clinicaId, false).Select(c => c.Id);
            return ContextoAgendamentos.Collection.Find(c => c.PacienteId == pacienteId && medicos.Contains(c.MedicoId)).ToList().OrderByDescending(c => c.DataAgendamento).ThenByDescending(c => c.HoraInicial).ToList();
        }

        internal List<Agendamento> BuscarPagamentoAgendamentoForma(string formaPagamentoId)
        {
            return ContextoAgendamentos.Collection.Find(c => c.Pagamentos.Any(d => d.FormaPagamentoId == formaPagamentoId)).ToList();
        }

        internal List<Agendamento> BuscarAgendamentoMedicoExcluir(string medicoId)
        {
            return ContextoAgendamentos.Collection.AsQueryable().Where(c => c.MedicoId == medicoId).Take(5).ToList();
        }

        internal List<Agendamento> ProcedimentosRealizados(DateTime dataInicio, DateTime dataFim, string medicoId)
        {
            var agendamentos = ContextoAgendamentos.Collection.Find(c => c.DataAgendamento >= dataInicio && c.DataAgendamento <= dataFim && (medicoId.IsNullOrWhiteSpace() || c.MedicoId == medicoId)
            && (c.TipoAgendamento == ETipoAgendamento.Cirurgia || c.TipoAgendamento == ETipoAgendamento.Exame || c.TipoAgendamento == ETipoAgendamento.Procedimento)).ToList().OrderBy(c => c.DataAgendamento).ToList();

            foreach (var agendamento in agendamentos)
            {
                agendamento.Paciente = RetornarPaciente(agendamento.PacienteId);
                agendamento.Medico = RetornarMedico(agendamento.MedicoId);
                agendamento.Clinica = RetornarClinica(agendamento.ClinicaId);
                agendamento.TipoAgendamentoDescricao = this.RetornarDescricaoAgendamento(agendamento);
            }

            return agendamentos;
        }

        internal Medico RetornarMedico(string medicoId)
        {
            var contextoMedico = new DbContexto<Medico>("medico");
            return contextoMedico.Collection.Find(c => c.Id == medicoId).FirstOrDefault();
        }

        internal Clinica RetornarClinica(string clinicaId)
        {
            var contextoClinica = new DbContexto<Clinica>("clinica");
            return contextoClinica.Collection.Find(c => c.Id == clinicaId).FirstOrDefault();
        }

        internal Paciente RetornarPaciente(string pacienteId)
        {
            var contextoPaciente = new DbContexto<Paciente>("paciente");

            return contextoPaciente.Collection.Find(c => c.Id == pacienteId).FirstOrDefault();
        }

        internal Convenio RetornarConvenio(string convenioId)
        {
            var contextoConvenio = new DbContexto<Convenio>("convenio");

            return contextoConvenio.Collection.Find(c => c.Id == convenioId).FirstOrDefault();
        }
    }
}