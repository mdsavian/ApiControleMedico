using System.Collections.Generic;
using System.Threading.Tasks;
using ApiControleMedico.Modelos;
using ApiControleMedico.Modelos.Enums;
using ApiControleMedico.Repositorio;
using ApiControleMedico.Uteis;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ApiControleMedico.Services
{
    public class UsuarioService
    {
        protected readonly DbContexto<Usuario> ContextoUsuario;
        protected readonly EntidadeNegocio<Usuario> UsuarioNegocio = new EntidadeNegocio<Usuario>();

        public UsuarioService()
        {
            ContextoUsuario = new DbContexto<Usuario>("usuario");
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            var usuarios = await UsuarioNegocio.GetAllAsync(ContextoUsuario.Collection);
            return usuarios;
        }

        public async Task<Usuario> CriarNovoUsuarioMedico(Medico medico)
        {
            var usuario = ContextoUsuario.Collection.Find(c => c.Login == medico.Email && c.MedicoId == medico.Id)
                .FirstOrDefault();

            if (usuario == null)
            {
                usuario = new Usuario
                {
                    Ativo = true,
                    Login = medico.Email,
                    Senha = Criptografia.Codifica("@medico1234"),
                    PermissaoAdministrador = true,
                    TipoUsuario = ETipoUsuario.Medico,
                    VisualizaValoresRelatorios = true,
                    MedicoId = medico.Id
                };
            }

            await UsuarioNegocio.SaveOneAsync(ContextoUsuario.Collection, usuario);
            return usuario;
        }

        public async Task<Usuario> CriarNovoUsuarioFuncionario(Funcionario funcionario)
        {
            var usuario = new Usuario
            {
                Ativo = true,
                Login = funcionario.Email,
                Senha = Criptografia.Codifica("@usuario1234"),
                PermissaoAdministrador = false,
                TipoUsuario = ETipoUsuario.Comum,
                VisualizaValoresRelatorios = false
            };

            await UsuarioNegocio.SaveOneAsync(ContextoUsuario.Collection, usuario);
            return usuario;
        }
    }
}