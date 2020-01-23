using System.Collections.Generic;
using System.Collections.ObjectModel;
using ApiControleMedico.Modelos;
using ApiControleMedico.Repositorio;
using MongoDB.Driver;

namespace ApiControleMedico.Services
{
    public class ExtraCaixaService
    {
        protected readonly DbContexto<ExtraCaixa> ContextoExtraCaixas;
        protected readonly EntidadeNegocio<ExtraCaixa> ExtraCaixaNegocio = new EntidadeNegocio<ExtraCaixa>();

        public ExtraCaixaService()
        {
            ContextoExtraCaixas = new DbContexto<ExtraCaixa>("extraCaixa");
        }

        public IEnumerable<ExtraCaixa> GetAll()
        {
            var extraCaixas = ExtraCaixaNegocio.GetAll(ContextoExtraCaixas.Collection);
            return extraCaixas;
        }

        public ExtraCaixa GetOne(string id)
        {
            return ExtraCaixaNegocio.GetOne(ContextoExtraCaixas.Collection, id);
        }

        public ExtraCaixa SaveOne(ExtraCaixa context)
        {
            ExtraCaixaNegocio.SaveOne(ContextoExtraCaixas.Collection, context);

            return context;
        }

        public bool RemoveOne(string id)
        {
            return ExtraCaixaNegocio.RemoveOne(ContextoExtraCaixas.Collection, id);
        }
    }
}