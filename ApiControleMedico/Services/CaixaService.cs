﻿using System;
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
            return caixas;
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

        public Caixa RetornarCaixaAbertoFuncionario(string funcionarioId)
        {
            return ContextoCaixas.Collection.Find(c =>
                    c.FuncionarioId == funcionarioId && (c.DataFechamento == null || c.DataFechamento == string.Empty))
                .FirstOrDefault();

        }

        public bool RemoveOne(string id)
        {
            return CaixaNegocio.RemoveOne(ContextoCaixas.Collection, id);
        }

        public List<Caixa> RetornarTodosCaixasAbertos()
        {
            return ContextoCaixas.Collection.AsQueryable()
                .Where(c => c.DataFechamento == null || c.DataFechamento == string.Empty).ToList();
        }
    }
}