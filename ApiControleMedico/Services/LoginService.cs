using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiControleMedico.Modelos;
using ApiControleMedico.Modelos.Enums;
using ApiControleMedico.Repositorio;
using ApiControleMedico.Uteis;
using MongoDB.Driver;

namespace ApiControleMedico.Services
{
    public class LoginService
    {
        protected readonly DbContexto<Usuario> ContextoUsuarios;
        protected readonly EntidadeNegocio<Usuario> UsuarioNegocio = new EntidadeNegocio<Usuario>();

        public LoginService()
        {
            ContextoUsuarios = new DbContexto<Usuario>("usuario");

        }

        private Usuario TratarUsuarioAdministrador(Usuario usuario)
        {
            if (Criptografia.Compara(usuario.Senha, Criptografia.Codifica("@adm1234")))
            {
                usuario.UltimoLogin = DateTime.Now.FormatarDiaMesAnoHora();
                usuario.Ativo = true;
                usuario.TipoUsuario = ETipoUsuario.Administrador;

                return usuario;
            }

            return null;
        }

        public Usuario ValidarLogin(Usuario usuario)
        {
            if (usuario.Login.Equals("admin"))
                return TratarUsuarioAdministrador(usuario);

            var usuarioBanco = ContextoUsuarios.Collection.Find(c => c.Login == usuario.Login && c.Ativo).FirstOrDefault();
            if (usuarioBanco != null && Criptografia.Compara(usuario.Senha, usuarioBanco.Senha))
            {
                usuarioBanco.UltimoLogin = DateTime.Now.FormatarDiaMesAnoHora();
                return usuarioBanco;
            }

            return null;

        }

        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            var usuarios = await UsuarioNegocio.GetAllAsync(ContextoUsuarios.Collection);
            return usuarios;
        }
    }
}