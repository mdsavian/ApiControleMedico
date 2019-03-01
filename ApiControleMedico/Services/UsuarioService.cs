﻿using System.Collections.Generic;
using System.Threading.Tasks;
using ApiControleMedico.Modelos;
using ApiControleMedico.Modelos.Enums;
using ApiControleMedico.Repositorio;
using ApiControleMedico.Uteis;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;

namespace ApiControleMedico.Services
{
    public class UsuarioService : ILogic<Usuario>
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

        public Task<IEnumerable<Usuario>> SaveManyAsync(IEnumerable<Usuario> contexts)
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

        public void CriarNovoUsuarioMedico(Medico medico)
        {
            var usuario = new Usuario
            {
                Ativo = true,
                Id = ObjectId.GenerateNewId().ToString(),
                Login = medico.Email,
                Senha = Criptografia.Codifica("1234"),
                PermissaoAdministrador = true,
                MedicoId = medico.Id,
                TipoUsuario = ETipoUsuario.Medico,
                VisualizaValoresRelatorios = true
            };
            Usuarios.Collection.InsertOne(usuario);
        }
    }
}