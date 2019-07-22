using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ApiControleMedico.Modelos;
using ApiControleMedico.Repositorio;
using ApiControleMedico.Uteis;
using Microsoft.AspNetCore.Mvc;
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
                    c.Medico.Id == medicoId).ToList().Where(c => c.DataAgendamento.ToDateTime() >= inicioSemana &&
                    c.DataAgendamento.ToDateTime() <= fimSemana).ToList();
            }
            catch (Exception ex)
            {
                return new List<Agendamento>();
            }
        }
    }
}