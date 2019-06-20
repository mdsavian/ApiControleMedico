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

        public async Task<IEnumerable<Agendamento>> GetAllAsync()
        {
            var agendamentos = await AgendamentoNegocio.GetAllAsync(ContextoAgendamentos.Collection);
            return agendamentos;
        }

        public Task<Agendamento> GetOneAsync(string id)
        {
            return AgendamentoNegocio.GetOneAsync(ContextoAgendamentos.Collection, id);
        }

        public async Task<Agendamento> SaveOneAsync(Agendamento context)
        {
            await AgendamentoNegocio.SaveOneAsync(ContextoAgendamentos.Collection, context);

            return context;
        }

        public Task<bool> RemoveOneAsync(string id)
        {
            return AgendamentoNegocio.RemoveOneAsync(ContextoAgendamentos.Collection, id);
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
                    c.Medico.Id == medicoId).ToList().Where(c=> c.DataAgendamento.ToDateTime() >= inicioSemana &&
                    c.DataAgendamento.ToDateTime() <= fimSemana).ToList();
            }
            catch (Exception ex)
            {
                return new List<Agendamento>();
            }
        }
    }
}