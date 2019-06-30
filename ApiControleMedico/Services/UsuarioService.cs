using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using ApiControleMedico.Modelos;
using ApiControleMedico.Modelos.Enums;
using ApiControleMedico.Modelos.NaoPersistidos;
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
                    TipoUsuario = ETipoUsuario.Medico,
                    MedicoId = medico.Id
                };
                await UsuarioNegocio.SaveOneAsync(ContextoUsuario.Collection, usuario);
            }
            return usuario;
        }

        public async Task<Usuario> CriarNovoUsuarioFuncionario(Funcionario funcionario)
        {
            var usuario = ContextoUsuario.Collection.Find(c => c.Login == funcionario.Email && c.FuncionarioId == funcionario.Id).FirstOrDefault();

            if (usuario == null)
            {
                usuario = new Usuario
                {
                    Ativo = true,
                    Login = funcionario.Email,
                    Senha = Criptografia.Codifica("@usuario1234"),
                    TipoUsuario = ETipoUsuario.Comum,
                    FuncionarioId = funcionario.Id
                };
                await UsuarioNegocio.SaveOneAsync(ContextoUsuario.Collection, usuario);
            }
            return usuario;
        }

        public Task<bool> RemoveOneAsync(string id)
        {
            return UsuarioNegocio.RemoveOneAsync(ContextoUsuario.Collection, id);
        }

        public async Task<Usuario> AlterarSenha(AlteraSenha alteraSenha)
        {
            var usuario = ContextoUsuario.Collection.Find(c => c.Id == alteraSenha.UsuarioId).FirstOrDefault();

            if (usuario != null)
            {
                if (!Criptografia.Compara(alteraSenha.SenhaAtual, usuario.Senha))
                    return null;

                usuario.Senha = Criptografia.Codifica(alteraSenha.ConfirmacaoNovaSenha);
                await UsuarioNegocio.SaveOneAsync(ContextoUsuario.Collection, usuario);

                return usuario;
            }
            return null;
        }
    }
}