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
        protected readonly DbContexto<Funcionario> Funcionarios;
        protected readonly EntidadeNegocio<Funcionario> FuncionarioNegocio = new EntidadeNegocio<Funcionario>();

        public FuncionarioService()
        {
            Funcionarios = new DbContexto<Funcionario>("Funcionario");
        }

        public async Task<IEnumerable<Funcionario>> GetAllAsync()
        {
            var funcionarios = await FuncionarioNegocio.GetAllAsync(Funcionarios.Collection);
            return funcionarios;
        }

        public Task<Funcionario> GetOneAsync(string id)
        {
            return FuncionarioNegocio.GetOneAsync(Funcionarios.Collection, id);
        }

        public async Task<Funcionario> SaveOneAsync(Funcionario funcionario)
        {
            await FuncionarioNegocio.SaveOneAsync(Funcionarios.Collection, funcionario);
            return funcionario;
        }

        public Task<bool> RemoveOneAsync(string id)
        {
            return FuncionarioNegocio.RemoveOneAsync(Funcionarios.Collection, id);
        }
    }
}