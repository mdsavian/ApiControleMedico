﻿using System.Collections.Generic;
using ApiControleMedico.Modelos;
using ApiControleMedico.Modelos.Enums;
using ApiControleMedico.Modelos.NaoPersistidos;
using ApiControleMedico.Repositorio;
using ApiControleMedico.Uteis;
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

        public IEnumerable<Usuario> GetAll()
        {
            var usuarios = UsuarioNegocio.GetAll(ContextoUsuario.Collection);
            return usuarios;
        }
        
        public Usuario CriarNovoUsuarioMedico(Medico medico)
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
                UsuarioNegocio.SaveOne(ContextoUsuario.Collection, usuario);
            }
            return usuario;
        }

        public Usuario CriarNovoUsuarioFuncionario(Funcionario funcionario)
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
                UsuarioNegocio.SaveOne(ContextoUsuario.Collection, usuario);
            }
            return usuario;
        }

        public bool RemoveOne(string id)
        {
            return UsuarioNegocio.RemoveOne(ContextoUsuario.Collection, id);
        }

        public Usuario AlterarSenha(AlteraSenha alteraSenha)
        {
            var usuario = ContextoUsuario.Collection.Find(c => c.Id == alteraSenha.UsuarioId).FirstOrDefault();

            if (usuario != null)
            {
                if (!Criptografia.Compara(alteraSenha.SenhaAtual, usuario.Senha))
                    return null;

                usuario.Senha = Criptografia.Codifica(alteraSenha.ConfirmacaoNovaSenha);
                UsuarioNegocio.SaveOne(ContextoUsuario.Collection, usuario);

                return usuario;
            }
            return null;
        }

        public Usuario GetOne(string id)
        {
            return UsuarioNegocio.GetOne(ContextoUsuario.Collection, id);
        }
    }
}