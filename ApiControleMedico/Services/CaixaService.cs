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
    public class CaixaService
    {
        protected readonly DbContexto<Caixa> ContextoCaixas;
        protected readonly EntidadeNegocio<Caixa> CaixaNegocio = new EntidadeNegocio<Caixa>();

        public CaixaService()
        {
            ContextoCaixas = new DbContexto<Caixa>("caixa");
        }

        public IEnumerable<Caixa> GetAll()
        {
            var caixas =  CaixaNegocio.GetAll(ContextoCaixas.Collection);
            return caixas.OrderByDescending(c => c.DataAbertura).ToList() ;
        }

        public Caixa GetOne(string id)
        {
            return CaixaNegocio.GetOne(ContextoCaixas.Collection, id);
        }

        public Caixa SaveOne(Caixa context)
        {
            CaixaNegocio.SaveOne(ContextoCaixas.Collection, context);

            return context;
        }

        public Caixa RetornarCaixaAbertoPessoa(string pessoaId)
        {
            return ContextoCaixas.Collection.Find(c =>
                    c.PessoaId == pessoaId && !c.DataFechamento.HasValue)
                .FirstOrDefault();

        }

        public bool RemoveOne(string id)
        {
            return CaixaNegocio.RemoveOne(ContextoCaixas.Collection, id);
        }

        public List<Caixa> RetornarTodosCaixasAbertos()
        {
            return ContextoCaixas.Collection.AsQueryable()
                .Where(c => !c.DataFechamento.HasValue).ToList();
        }
    }
}