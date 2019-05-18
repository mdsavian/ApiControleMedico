using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ApiControleMedico.Modelos;
using ApiControleMedico.Repositorio;
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

    }
}