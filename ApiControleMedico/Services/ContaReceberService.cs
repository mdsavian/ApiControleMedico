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
            return contaRecebers.OrderByDescending(c=> c.DataEmissao).ToList();
        }

        public ContaReceber GetOne(string id)
        {
            var contaReceber = ContaReceberNegocio.GetOne(ContextoContasReceber.Collection, id);
            if (!contaReceber.AgendamentoId.IsNullOrWhiteSpace())
            {
                var agendamentoService = new AgendamentoService();
                var agendamento = agendamentoService.GetOne(contaReceber.AgendamentoId);
                contaReceber.TipoContaDescricao = agendamentoService.RetornarDescricaoAgendamento(agendamento);
            }
            else contaReceber.TipoContaDescricao = "Lançamento Manual";

            return contaReceber;
        }

        internal ActionResult<ContaReceber> BuscarPorAgendamento(string agendamentoId)
        {            
            return this.BuscarAgendamentosParaListagem(agendamentoId).FirstOrDefault();
        }

        public ContaReceber SaveOne(ContaReceber context)
        {
            ContaReceberNegocio.SaveOne(ContextoContasReceber.Collection, context);

            return context;
        }

        private List<ContaReceber> BuscarAgendamentosParaListagem(string agendamentoId = "")
        {
            var lista = new List<ContaReceber>();
            var agendamentoService = new AgendamentoService();

            var agendamentos = agendamentoService.GetAll().Where(c => c.ContemPagamentos && (agendamentoId == "" || c.Id == agendamentoId)).ToList();

            foreach (var agendamento in agendamentos)
            {
                int documento = int.Parse(agendamento.HoraInicial) + int.Parse(agendamento.HoraFinal) + (agendamento.Pagamentos.HasItems()? decimal.ToInt32(Math.Truncate(agendamento.Pagamentos.Sum(c=> c.Valor))) : 0);

                var contaReceber = new ContaReceber
                {
                    AgendamentoId = agendamento.Id,
                    DataEmissao = agendamento.DataAgendamento,
                    DataVencimento = agendamento.DataAgendamento.AddDays(30),
                    ClinicaId = agendamento.ClinicaId,
                    Valor = agendamento.Pagamentos.Sum(c => c.Valor * c.Parcela),
                    ValorTotal = agendamento.Pagamentos.Sum(c => c.Valor * c.Parcela),
                    PacienteId = agendamento.PacienteId,
                    MedicoId = agendamento.MedicoId,
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

            return lista.OrderByDescending(c => c.DataEmissao).ToList();
        }

        public bool RemoveOne(string id)
        {
            return ContaReceberNegocio.RemoveOne(ContextoContasReceber.Collection, id);
        }

        internal List<ContaReceber> TodosPorPeriodo(DateTime primeiroDiaMes, DateTime dataHoje, string medicoId, string funcionarioId)
        {
            string usuarioId = "";
            if (!funcionarioId.IsNullOrWhiteSpace())
                usuarioId = new UsuarioService().GetAll().FirstOrDefault(c => c.FuncionarioId == funcionarioId)?.Id;

            var ts = new TimeSpan(23, 59, 59);
            dataHoje = dataHoje + ts;

            return ContextoContasReceber.Collection.Find(c =>
                c.DataEmissao >= primeiroDiaMes && c.DataEmissao <= dataHoje &&
                (medicoId.IsNullOrWhiteSpace() || c.MedicoId == medicoId || c.MedicoId == "") && (usuarioId.IsNullOrWhiteSpace() || c.UsuarioId == usuarioId)).ToList();
        }
    }
}