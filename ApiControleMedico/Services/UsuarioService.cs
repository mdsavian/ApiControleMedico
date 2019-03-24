using System.Collections.Generic;
using System.Threading.Tasks;
using ApiControleMedico.Modelos;
using ApiControleMedico.Modelos.Enums;
using ApiControleMedico.Repositorio;
using ApiControleMedico.Uteis;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;

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

        public void CriarNovoUsuarioMedico(Medico medico)
        {
            var usuario = new Usuario
            {
                Ativo = true,
                Id = ObjectId.GenerateNewId().ToString(),
                Login = medico.Email,
                Senha = Criptografia.Codifica("@medico1234"),
                PermissaoAdministrador = true,
                Medico = medico,
                TipoUsuario = ETipoUsuario.Medico,
                VisualizaValoresRelatorios = true
            };
            ContextoUsuario.Collection.InsertOne(usuario);

            medico.Usuario = usuario;
        }

        public async Task<Usuario> CriarNovoUsuarioFuncionario(Funcionario funcionario)
        {
            var usuario = new Usuario
            {
                Ativo = true,
                Login = funcionario.Email,
                Senha = Criptografia.Codifica("@usuario1234"),
                PermissaoAdministrador = false,
                Funcionario = funcionario,
                TipoUsuario = ETipoUsuario.Comum,
                VisualizaValoresRelatorios = false
            };

            //await UsuarioNegocio.SaveOneAsync(ContextoUsuario.Collection, usuario);
            return usuario;
        }
    }
}