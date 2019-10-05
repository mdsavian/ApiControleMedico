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
            return agendamentos;
        }

        public Agendamento GetOne(string id)
        {
            return AgendamentoNegocio.GetOne(ContextoAgendamentos.Collection, id);
        }

        public Agendamento SaveOne(Agendamento context)
        {
            AgendamentoNegocio.SaveOne(ContextoAgendamentos.Collection, context);

            return context;
        }

        public bool RemoveOne(string id)
        {
            return AgendamentoNegocio.RemoveOne(ContextoAgendamentos.Collection, id);
        }

        public List<Agendamento> BuscarAgendamentoMedico(string medicoId, string data, string tipoCalendario)
        {
            try
            {
                var inicioSemana = data.ToDateTime();
                var fimSemana = inicioSemana;

                if (tipoCalendario == "week")
                {
                    inicioSemana = data.ToDateTime().InicioDaSemana();
                    fimSemana = inicioSemana.AddDays(6);
                }

                return ContextoAgendamentos.Collection.Find(c =>
                    c.MedicoId == medicoId).ToList().Where(c => c.DataAgendamento.ToDateTime() >= inicioSemana &&
                    c.DataAgendamento.ToDateTime() <= fimSemana).ToList();
            }
            catch (Exception ex)
            {
                return new List<Agendamento>();
            }
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

        internal List<Agendamento> BuscarAgendamentosProcedimento(string procedimentoId)
        {
            return ContextoAgendamentos.Collection.Find(c => c.ProcedimentoId == procedimentoId).ToList();
        }

        internal List<Agendamento> BuscarAgendamentosLocal(string localId)
        {
            return ContextoAgendamentos.Collection.Find(c => c.LocalId == localId).ToList();
        }

        internal List<Agendamento> BuscarAgendamentosPaciente(string pacienteId)
        {
            return ContextoAgendamentos.Collection.Find(c => c.PacienteId == pacienteId).ToList();
        }

        internal List<Agendamento> BuscarPagamentoAgendamentoForma(string formaPagamentoId)
        {
            return ContextoAgendamentos.Collection.Find(c => c.Pagamentos.Any(d=> d.FormaPagamentoId == formaPagamentoId)).ToList();
        }

        internal List<Agendamento> BuscarAgendamentoMedicoExcluir(string medicoId)
        {            
            return ContextoAgendamentos.Collection.AsQueryable().Where(c => c.MedicoId == medicoId).Take(5).ToList();
        }
    }
}