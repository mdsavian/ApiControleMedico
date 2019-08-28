using System.Collections.Generic;
using ApiControleMedico.Modelos;
using ApiControleMedico.Repositorio;

namespace ApiControleMedico.Services
{
    public class AgendamentoPagamentoService: IService
    {
        protected readonly DbContexto<AgendamentoPagamento> ContextoAgendamentoPagamentos;
        protected readonly EntidadeNegocio<AgendamentoPagamento> AgendamentoPagamentoNegocio = new EntidadeNegocio<AgendamentoPagamento>();

        public AgendamentoPagamentoService()
        {
            ContextoAgendamentoPagamentos = new DbContexto<AgendamentoPagamento>("agendamentoPagamento");
        }

        public IEnumerable<AgendamentoPagamento> GetAll()
        {
            var agendamentoPagamentos = AgendamentoPagamentoNegocio.GetAll(ContextoAgendamentoPagamentos.Collection);
            return agendamentoPagamentos;
        }

        public AgendamentoPagamento GetOne(string id)
        {
            return AgendamentoPagamentoNegocio.GetOne(ContextoAgendamentoPagamentos.Collection, id);
        }

        public AgendamentoPagamento SaveOne(AgendamentoPagamento context)
        {
            AgendamentoPagamentoNegocio.SaveOne(ContextoAgendamentoPagamentos.Collection, context);

            return context;
        }

        public bool RemoveOne(string id)
        {
            return AgendamentoPagamentoNegocio.RemoveOne(ContextoAgendamentoPagamentos.Collection, id);
        }

    }
}