using System;
using System.Collections.Generic;
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

        private Usuario TratarUsuarioAdministrador(string login, string senha)
        {
            if (Criptografia.Compara(senha, Criptografia.Codifica("@adm1234")))
            {
                return new Usuario
                {
                    Login = login,
                    Senha = senha,
                    UltimoLogin = DateTime.Now.FormatarDiaMesAnoHora(),
                    Ativo = true,
                    SessaoAtiva = true,
                    TipoUsuario = ETipoUsuario.Administrador
                };                
            }

            return null;
        }

        public Usuario ValidarLogin(string login, string senha)
        {
            if (login.Equals("admin"))
                return TratarUsuarioAdministrador(login, senha);

            var usuario = ContextoUsuarios.Collection.Find(c => c.Login == login && c.Ativo).FirstOrDefault();

            if (usuario != null)
            {
                if (Criptografia.Compara(senha, usuario.Senha))
                {
                    usuario = new UsuarioService().BuscarUsuarioComModelos(usuario.Id, senha);
                    usuario.UltimoLogin = DateTime.Now.FormatarDiaMesAnoHora();
                    usuario.SessaoAtiva = true;
                }
                else return null;
            }
            return usuario;
        }

        public bool ValidarSenha(Usuario usuario)
        {
            var usuarioBanco = ContextoUsuarios.Collection.Find(c => c.Login == usuario.Login && c.Ativo).FirstOrDefault();
            return usuarioBanco != null && Criptografia.Compara(usuario.Senha, usuarioBanco.Senha);
        }

        public IEnumerable<Usuario> GetAll()
        {
            var usuarios = UsuarioNegocio.GetAll(ContextoUsuarios.Collection);
            return usuarios;
        }
    }
}