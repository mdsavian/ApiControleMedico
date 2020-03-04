using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ApiControleMedico.Modelos;
using ApiControleMedico.Repositorio;
using ApiControleMedico.Uteis;
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
        
        public List<ExtraCaixa> BuscarPorCaixa(string caixaId)
        {
            return ContextoExtraCaixas.Collection.Find(c => c.CaixaId == caixaId).ToList();
        }

        internal List<ExtraCaixa> TodosPorPeriodo(DateTime dataInicio, DateTime dataFim, string medicoId, string caixaId, string funcionarioId)
        {
            string usuarioId = "";
            if (!funcionarioId.IsNullOrWhiteSpace())
                usuarioId = new UsuarioService().GetAll().FirstOrDefault(c => c.FuncionarioId == funcionarioId)?.Id;
            
            var ts = new TimeSpan(23,59,59);
            dataFim = dataFim + ts;

            var extras = ContextoExtraCaixas.Collection.Find(c => c.Data >= dataInicio && c.Data <= dataFim && (medicoId.IsNullOrWhiteSpace() || c.MedicoId == medicoId)
                                                            && (caixaId.IsNullOrWhiteSpace() || c.CaixaId == caixaId)
                                                            && (usuarioId.IsNullOrWhiteSpace() || c.UsuarioId == usuarioId)).ToList();
            var caixaService = new CaixaService();
            foreach (var extra in extras)
            {
                if (!extra.CaixaId.IsNullOrWhiteSpace())
                    extra.Caixa = caixaService.GetOne(extra.CaixaId);
            }

            return extras;
        }
    }
}