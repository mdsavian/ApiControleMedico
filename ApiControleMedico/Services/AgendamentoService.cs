using System;
using System.Collections.Generic;
using System.Linq;
using ApiControleMedico.Modelos;
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
            }
            return agendamentos;
        }

        public Agendamento GetOne(string id)
        {
            return AgendamentoNegocio.GetOne(ContextoAgendamentos.Collection, id);
        }

        public Agendamento SaveOne(Agendamento agendamento)
        {

            var offset = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow);
            agendamento.DataAgendamento = new DateTimeOffset(agendamento.DataAgendamento).ToOffset(offset).DateTime;

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

            var contextoPaciente = new DbContexto<Paciente>("paciente");
            var contextoMedico = new DbContexto<Medico>("medico");
            var contextoConvenio= new DbContexto<Convenio>("convenio");
            foreach (var agendamento in agendamentos)
            {
                agendamento.Paciente = contextoPaciente.Collection.Find(c => c.Id == agendamento.PacienteId).FirstOrDefault();
                agendamento.Medico = contextoMedico.Collection.Find(c => c.Id == agendamento.MedicoId).FirstOrDefault();
                agendamento.Convenio = contextoConvenio.Collection.Find(c => c.Id == agendamento.ConvenioId).FirstOrDefault();

                agendamento.TipoAgendamentoDescricao = this.RetornarDescricaoAgendamento(agendamento);
            }
            contextoPaciente.Dispose();
            contextoMedico.Dispose();
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

        internal List<Agendamento> TodosPorPeriodo(DateTime primeiroDiaMes, DateTime dataHoje, string medicoId)
        {
            return ContextoAgendamentos.Collection.Find(c => c.DataAgendamento >= primeiroDiaMes && c.DataAgendamento <= dataHoje && (medicoId.IsNullOrWhiteSpace() || c.MedicoId == medicoId)).ToList();
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
            var medicos = new MedicoService().BuscarMedicosPorUsuario(usuarioId, clinicaId, false).Select(c=> c.Id);
            return ContextoAgendamentos.Collection.Find(c => c.PacienteId == pacienteId && medicos.Contains(c.MedicoId)).ToList().OrderByDescending(c=> c.DataAgendamento).ThenByDescending(c=> c.HoraInicial).ToList();
        }

        internal List<Agendamento> BuscarPagamentoAgendamentoForma(string formaPagamentoId)
        {
            return ContextoAgendamentos.Collection.Find(c => c.Pagamentos.Any(d => d.FormaPagamentoId == formaPagamentoId)).ToList();
        }

        internal List<Agendamento> BuscarAgendamentoMedicoExcluir(string medicoId)
        {
            return ContextoAgendamentos.Collection.AsQueryable().Where(c => c.MedicoId == medicoId).Take(5).ToList();
        }
    }
}