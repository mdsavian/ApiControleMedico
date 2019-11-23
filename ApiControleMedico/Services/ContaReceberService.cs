using System;
using System.Collections.Generic;
using System.Linq;
using ApiControleMedico.Modelos;
using ApiControleMedico.Repositorio;
using ApiControleMedico.Uteis;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace ApiControleMedico.Services
{
    public class ContaReceberService
    {
        protected readonly DbContexto<ContaReceber> ContextoContasReceber;
        protected readonly EntidadeNegocio<ContaReceber> ContaReceberNegocio = new EntidadeNegocio<ContaReceber>();

        public ContaReceberService()
        {
            ContextoContasReceber = new DbContexto<ContaReceber>("contaReceber");
        }

        public IEnumerable<ContaReceber> GetAll()
        {
            var contaRecebers = ContaReceberNegocio.GetAll(ContextoContasReceber.Collection).ToList();
            foreach (var conta in contaRecebers)
            {
                conta.TipoContaDescricao = "Lançamento Manual";
            }
            contaRecebers.AddRange(this.BuscarAgendamentosParaListagem());
            return contaRecebers;
        }

        public ContaReceber GetOne(string id)
        {
            return ContaReceberNegocio.GetOne(ContextoContasReceber.Collection, id);
        }

        public ContaReceber SaveOne(ContaReceber context)
        {
            ContaReceberNegocio.SaveOne(ContextoContasReceber.Collection, context);

            return context;
        }

        private List<ContaReceber> BuscarAgendamentosParaListagem()
        {
            var lista = new List<ContaReceber>();
            var agendamentoService = new AgendamentoService();

            var agendamentos = agendamentoService.GetAll().Where(c => c.ContemPagamentos).ToList();

            foreach (var agendamento in agendamentos)
            {
                int documento = int.Parse(agendamento.HoraInicial) + int.Parse(agendamento.HoraFinal) + decimal.ToInt32(Math.Truncate(agendamento.Pagamentos.Sum(c=> c.Valor)));

                var contaReceber = new ContaReceber
                {
                    AgendamentoId = agendamento.Id,
                    DataEmissao = agendamento.DataAgendamento,
                    DataVencimento = agendamento.DataAgendamento.AddDays(30),
                    ClinicaId = agendamento.ClinicaId,
                    Valor = agendamento.Pagamentos.Sum(c => c.Valor * c.Parcela),
                    ValorTotal = agendamento.Pagamentos.Sum(c => c.Valor * c.Parcela),
                    PacienteId = agendamento.PacienteId,
                    NumeroDocumento =  documento.ToString(),
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
                        Valor = pagamento.Valor ,
                        VistaPrazo = pagamento.VistaPrazo,
                        UsuarioId = pagamento.UsuarioId
                    });
                }

                contaReceber.TipoContaDescricao = agendamentoService.RetornarDescricaoAgendamento(agendamento);

                lista.Add(contaReceber);

            }


            return lista;
        }

        public bool RemoveOne(string id)
        {
            return ContaReceberNegocio.RemoveOne(ContextoContasReceber.Collection, id);
        }

        internal List<ContaReceber> TodosPorPeriodo(DateTime primeiroDiaMes, DateTime dataHoje, string medicoId)
        {
            return ContextoContasReceber.Collection.Find(c => c.DataEmissao >= primeiroDiaMes && c.DataEmissao <= dataHoje && (medicoId.IsNullOrWhiteSpace() || c.MedicoId == medicoId)).ToList();
        }
    }
}