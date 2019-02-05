using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiControleMedico.Controllers;
using ApiControleMedico.Modelos;
using ApiControleMedico.Repositorio;
using ApiControleMedico.Uteis;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ApiControleMedico.Services
{
    public class LoginService : ILogic<Usuario>
    {
        protected readonly DbContexto<Usuario> Usuarios;
        protected readonly EntidadeNegocio<Usuario, Usuario> UsuarioNegocio = new EntidadeNegocio<Usuario, Usuario>();

        public LoginService()
        {
            Usuarios = new DbContexto<Usuario>("usuario");

            ValidaUsuarioAdministrador();
        }

        private void ValidaUsuarioAdministrador()
        {
            if (Usuarios.Collection.Find(c => c.Login == "admin").FirstOrDefault() == null)
            {
                var usuario = new Usuario
                {
                    Login = "admin",
                    Senha = Criptografia.Codifica("1234"),
                    Ativo = true
                };
                Usuarios.Collection.InsertOne(usuario);
            }
        }

        public Usuario ValidarLogin(Usuario usuario)
        {
            var usuarioBanco = Usuarios.Collection.Find(c => c.Login == usuario.Login && c.Ativo).FirstOrDefault();
            if (usuarioBanco != null && Criptografia.Compara(usuario.Senha, usuarioBanco.Senha))
            {
                usuarioBanco.Token = $"{DateTime.Now}{usuario.Login}{Guid.NewGuid()}";
                return usuarioBanco;
            }

            return null;

        }
        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            var usuarios = await UsuarioNegocio.GetAllAsync(Usuarios.Collection);
            return usuarios;
        }

        public Task<Usuario> GetOneAsync(Usuario context)
        {
            throw new System.NotImplementedException();
        }

        public Task<Usuario> GetOneAsync(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<Usuario> GetManyAsync(IEnumerable<Usuario> contexts)
        {
            throw new System.NotImplementedException();
        }

        public Task<Usuario> GetManyAsync(IEnumerable<string> ids)
        {
            throw new System.NotImplementedException();
        }

        public Task<Usuario> SaveOneAsync(Usuario Context)
        {
            throw new System.NotImplementedException();
        }

        public Task<Usuario> SaveManyAsync(IEnumerable<Usuario> contexts)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> RemoveOneAsync(Usuario context)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> RemoveOneAsync(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> RemoveManyAsync(IEnumerable<Usuario> contexts)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> RemoveManyAsync(IEnumerable<string> ids)
        {
            throw new System.NotImplementedException();
        }
    }
}