using System;
using System.Collections.Generic;
using ApiControleMedico.Modelos;
using ApiControleMedico.Repositorio;

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

            var usuario = new UsuarioService().CriarNovoUsuarioFuncionario(funcionario);
            funcionario.UsuarioId = usuario.Id;

            FuncionarioNegocio.SaveOne(ContextoFuncionarios.Collection, funcionario);
            return funcionario;
        }

        public bool RemoveOne(string id)
        {
            var usuarioService = new UsuarioService();
            var medico = FuncionarioNegocio.GetOne(ContextoFuncionarios.Collection, id);
            usuarioService.RemoveOne(medico.Usuario.Id);

            return FuncionarioNegocio.RemoveOne(ContextoFuncionarios.Collection, id);
        }
    }
}