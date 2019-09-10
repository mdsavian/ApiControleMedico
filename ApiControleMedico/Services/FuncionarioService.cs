using System;
using System.Collections.Generic;
using ApiControleMedico.Modelos;
using ApiControleMedico.Repositorio;
using ApiControleMedico.Uteis;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace ApiControleMedico.Services
{
    public class FuncionarioService
    {
        protected readonly DbContexto<Funcionario> ContextoFuncionarios;
        protected readonly EntidadeNegocio<Funcionario> FuncionarioNegocio = new EntidadeNegocio<Funcionario>();

        public FuncionarioService()
        {
            ContextoFuncionarios = new DbContexto<Funcionario>("funcionario");
        }

        public IEnumerable<Funcionario> GetAll()
        {
            var funcionarios = FuncionarioNegocio.GetAll(ContextoFuncionarios.Collection);
            return funcionarios;
        }

        public Funcionario GetOne(string id)
        {
            return FuncionarioNegocio.GetOne(ContextoFuncionarios.Collection, id);
        }

        public Funcionario SaveOne(Funcionario funcionario)
        {
            FuncionarioNegocio.SaveOne(ContextoFuncionarios.Collection, funcionario);
            if (funcionario.UsuarioId.IsNullOrWhiteSpace())
            {
                var usuario = new UsuarioService().CriarNovoUsuarioFuncionario(funcionario);
                funcionario.UsuarioId = usuario.Id;
            }

            FuncionarioNegocio.SaveOne(ContextoFuncionarios.Collection, funcionario);
            return funcionario;
        }

        public bool RemoveOne(string id)
        {
            var usuarioService = new UsuarioService();
            var func = FuncionarioNegocio.GetOne(ContextoFuncionarios.Collection, id);
            usuarioService.RemoveOne(func.UsuarioId);

            return FuncionarioNegocio.RemoveOne(ContextoFuncionarios.Collection, id);
        }

        public Funcionario BuscarComMedicos(string funcionarioId)
        {
            var funcionario = FuncionarioNegocio.GetOne(ContextoFuncionarios.Collection, funcionarioId);

            if (funcionario.MedicosId.HasItems())
            {
                var medicoService = new MedicoService();
                funcionario.Medicos = new List<Medico>();

                foreach (var medicoId in funcionario.MedicosId)
                    funcionario.Medicos.Add(medicoService.GetOne(medicoId));
            }

            return funcionario;
        }

        public List<Funcionario> BuscarPorOficio(string oficioId)
        {
            return ContextoFuncionarios.Collection.Find(c => c.OficioId == oficioId).ToList();           
        }
    }
}