using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ApiControleMedico.Modelos;
using ApiControleMedico.Repositorio;
using MongoDB.Driver;

namespace ApiControleMedico.Services
{
    public class FormaDePagamentoService
    {
        protected readonly DbContexto<FormaDePagamento> ContextoFormaDePagamentos;
        protected readonly EntidadeNegocio<FormaDePagamento> FormaDePagamentoNegocio = new EntidadeNegocio<FormaDePagamento>();

        public FormaDePagamentoService()
        {
            ContextoFormaDePagamentos = new DbContexto<FormaDePagamento>("formaDePagamento");
        }

        public IEnumerable<FormaDePagamento> GetAll()
        {
            var formaDePagamentos = FormaDePagamentoNegocio.GetAll(ContextoFormaDePagamentos.Collection);
            return formaDePagamentos;
        }

        public FormaDePagamento GetOne(string id)
        {
            return FormaDePagamentoNegocio.GetOne(ContextoFormaDePagamentos.Collection, id);
        }

        public FormaDePagamento SaveOne(FormaDePagamento context)
        {
            FormaDePagamentoNegocio.SaveOne(ContextoFormaDePagamentos.Collection, context);

            return context;
        }

        public bool RemoveOne(string id)
        {
            return FormaDePagamentoNegocio.RemoveOne(ContextoFormaDePagamentos.Collection, id);
        }

        public void SalvarDadosFixos(Collection<FormaDePagamento> formaDePagamentos)
        {
            foreach (var formaDePagamento in formaDePagamentos)
            {
                if (ContextoFormaDePagamentos.Collection.Find(c => c.Descricao == formaDePagamento.Descricao)
                        .FirstOrDefault() != null)
                    continue;

                FormaDePagamentoNegocio.SaveOne(ContextoFormaDePagamentos.Collection, formaDePagamento);
            }
        }
    }
}