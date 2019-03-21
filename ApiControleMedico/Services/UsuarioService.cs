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
        protected readonly DbContexto<Usuario> Usuarios;
        protected readonly EntidadeNegocio<Usuario> UsuarioNegocio = new EntidadeNegocio<Usuario>();

        public UsuarioService()
        {
            Usuarios = new DbContexto<Usuario>("usuario");
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            var usuarios = await UsuarioNegocio.GetAllAsync(Usuarios.Collection);
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
            Usuarios.Collection.InsertOne(usuario);

            medico.Usuario = usuario;
        }

        public void CriarNovoUsuarioFuncionario(Funcionario funcionario)
        {
            var usuario = new Usuario
            {
                Ativo = true,
                Id = ObjectId.GenerateNewId().ToString(),
                Login = funcionario.Email,
                Senha = Criptografia.Codifica("@usuario1234"),
                PermissaoAdministrador = false,
                Funcionario = funcionario,
                TipoUsuario = ETipoUsuario.Comum,
                VisualizaValoresRelatorios = false
            };
            Usuarios.Collection.InsertOne(usuario);

            funcionario.Usuario = usuario;
        }
    }
}