using System;
using System.Collections.Generic;
using System.Linq;
using ApiControleMedico.Modelos;
using ApiControleMedico.Repositorio;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace ApiControleMedico.Services
{
    public class ContaReceberService
    {
        protected readonly DbContexto<ContaReceber> ContextoContaRecebers;
        protected readonly EntidadeNegocio<ContaReceber> ContaReceberNegocio = new EntidadeNegocio<ContaReceber>();

        public ContaReceberService()
        {
            ContextoContaRecebers = new DbContexto<ContaReceber>("contaReceber");
        }

        public IEnumerable<ContaReceber> GetAll()
        {
            var contaRecebers = ContaReceberNegocio.GetAll(ContextoContaRecebers.Collection).ToList();
            contaRecebers.AddRange(this.BuscarAgendamentosParaListagem());
            return contaRecebers;
        }

        public ContaReceber GetOne(string id)
        {
            return ContaReceberNegocio.GetOne(ContextoContaRecebers.Collection, id);
        }

        public ContaReceber SaveOne(ContaReceber context)
        {
            ContaReceberNegocio.SaveOne(ContextoContaRecebers.Collection, context);

            return context;
        }

        private List<ContaReceber> BuscarAgendamentosParaListagem()
        {
            var lista = new List<ContaReceber>();

            var agendamentos = new AgendamentoService().GetAll().Where(c => c.ContemPagamentos).ToList();

            foreach (var agendamento in agendamentos)
            {
                var contaReceber = new ContaReceber
                {
                    AgendamentoId = agendamento.Id,
                    DataEmissao = agendamento.DataAgendamento,
                    DataVencimento = agendamento.DataAgendamento.AddDays(30),
                    ClinicaId = agendamento.ClinicaId,
                    Valor = agendamento.Pagamentos.Sum(c => c.Valor),
                    ValorTotal = agendamento.Pagamentos.Sum(c => c.Valor),
                    PacienteId = agendamento.PacienteId,
                    NumeroDocumento = agendamento.Id,
                    Pagamentos = new List<ContaReceberPagamento>()
                };

                foreach (var pagamento in agendamento.Pagamentos)
                {
                    var i = 0;
                    contaReceber.Pagamentos.Add(new ContaReceberPagamento
                    {
                        Codigo = i++,
                        DataPagamento = agendamento.DataAgendamento,
                        FormaPagamentoId = pagamento.FormaPagamentoId,
                        Parcela = pagamento.Parcela,
                        Valor = pagamento.Valor,
                        VistaPrazo = pagamento.VistaPrazo,
                        UsuarioId = pagamento.UsuarioId
                    });
                }

                if (!string.IsNullOrWhiteSpace(agendamento.ExameId))
                {
                    contaReceber.TipoContaDescricao = new ExameService().GetOne(agendamento.ExameId)?.Descricao;
                }
                else if (!string.IsNullOrWhiteSpace(agendamento.CirurgiaId))
                {
                    contaReceber.TipoContaDescricao = new CirurgiaService().GetOne(agendamento.CirurgiaId)?.Descricao;
                }
                else if (!string.IsNullOrWhiteSpace(agendamento.ProcedimentoId))
                {
                    contaReceber.TipoContaDescricao = new ProcedimentoService().GetOne(agendamento.ProcedimentoId)?.Descricao;
                }
                else 
                {
                    contaReceber.TipoContaDescricao = agendamento.TipoAgendamento.ToString();
                }

                lista.Add(contaReceber);

            }


            return lista;
        }

        public bool RemoveOne(string id)
        {
            return ContaReceberNegocio.RemoveOne(ContextoContaRecebers.Collection, id);
        }
    }
}