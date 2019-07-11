using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiControleMedico.Modelos;
using ApiControleMedico.Repositorio;
using ApiControleMedico.Uteis;

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

        public async Task<IEnumerable<Funcionario>> GetAllAsync()
        {
            var funcionarios = await FuncionarioNegocio.GetAllAsync(ContextoFuncionarios.Collection);
            return funcionarios;
        }

        public Task<Funcionario> GetOneAsync(string id)
        {
            return FuncionarioNegocio.GetOneAsync(ContextoFuncionarios.Collection, id);
        }

        public async Task<Funcionario> SaveOneAsync(Funcionario funcionario)
        {
            await FuncionarioNegocio.SaveOneAsync(ContextoFuncionarios.Collection, funcionario);

            var usuario = await new UsuarioService().CriarNovoUsuarioFuncionario(funcionario);
            funcionario.UsuarioId = usuario.Id;

            await FuncionarioNegocio.SaveOneAsync(ContextoFuncionarios.Collection, funcionario);
            return funcionario;
        }

        public Task<bool> RemoveOneAsync(string id)
        {
            var usuarioService = new UsuarioService();
            var medico = FuncionarioNegocio.GetOneAsync(ContextoFuncionarios.Collection, id).Result;
            usuarioService.RemoveOneAsync(medico.Usuario.Id);

            return FuncionarioNegocio.RemoveOneAsync(ContextoFuncionarios.Collection, id);
        }
    }
}