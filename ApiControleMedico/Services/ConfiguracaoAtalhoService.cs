using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ApiControleMedico.Modelos;
using ApiControleMedico.Repositorio;
using ApiControleMedico.Uteis;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace ApiControleMedico.Services
{
    public class ConfiguracaoAtalhoService
    {
        protected readonly DbContexto<ConfiguracaoAtalho> ContextoConfiguracaoAtalhos;
        protected readonly EntidadeNegocio<ConfiguracaoAtalho> ConfiguracaoAtalhoNegocio = new EntidadeNegocio<ConfiguracaoAtalho>();

        public ConfiguracaoAtalhoService()
        {
            ContextoConfiguracaoAtalhos = new DbContexto<ConfiguracaoAtalho>("configuracaoAtalho");
        }

        public List<ConfiguracaoAtalho> GetAll()
        {
            var configuracaoAtalhos = ConfiguracaoAtalhoNegocio.GetAll(ContextoConfiguracaoAtalhos.Collection);
            return configuracaoAtalhos.ToList();
        }

        public ConfiguracaoAtalho GetOne(string id)
        {
            return ConfiguracaoAtalhoNegocio.GetOne(ContextoConfiguracaoAtalhos.Collection, id);
        }

        public ConfiguracaoAtalho SaveOne(ConfiguracaoAtalho context)
        {
            ConfiguracaoAtalhoNegocio.SaveOne(ContextoConfiguracaoAtalhos.Collection, context);
            return context;
        }

        public List<ConfiguracaoAtalho> BuscarPorUsuario(string usuarioId)
        {
            var configuracoes = new List<ConfiguracaoAtalho>();

            var usuario = new UsuarioService().GetOne(usuarioId);

            if (usuario.ConfiguracaoAtalhos.HasItems())
                return usuario.ConfiguracaoAtalhos.Where(c=> c.Ativo).ToList();

            else return BuscarParaConfiguracao(usuarioId);
        }

        public List<ConfiguracaoAtalho> BuscarParaConfiguracao(string usuarioId)
        {
            var configuracoes = new List<ConfiguracaoAtalho>();

            var usuario = new UsuarioService().GetOne(usuarioId);

            if (usuario.ConfiguracaoAtalhos.HasItems())
                return usuario.ConfiguracaoAtalhos.ToList();

            Funcionario funcionario = null;
            Medico medico = null;

            if (!usuario.FuncionarioId.IsNullOrWhiteSpace())
                funcionario = new FuncionarioService().GetOne(usuario.FuncionarioId);
            else if (!usuario.MedicoId.IsNullOrWhiteSpace())
                medico = new MedicoService().GetOne(usuario.MedicoId);

            if (medico != null || (funcionario != null && funcionario.VisualizaAgenda))
                configuracoes.Add(new ConfiguracaoAtalho
                {
                    Descricao = "Agenda",
                    BtnClass = "btn-danger btn btn-lg btn-home",
                    SpanClass = "fa fa-3x fa-calendar"
                });

            if (medico != null || (funcionario != null && funcionario.PermissaoAdministrador))
                configuracoes.Add(new ConfiguracaoAtalho
                {
                    Descricao = "Dashboard",
                    BtnClass = "btn-warning btn btn-lg btn-home",
                    SpanClass = "fa fa-3x fa-area-chart"
                });

            configuracoes.Add(new ConfiguracaoAtalho
            {
                Descricao = "Pacientes",
                BtnClass = "btn-primary btn btn-lg btn-home",
                SpanClass = "fa fa-3x fa-users"
            });

            if (medico != null || (funcionario != null && funcionario.PermissaoAdministrador))
                configuracoes.Add(new ConfiguracaoAtalho
                {
                    Descricao = "Médicos",
                    BtnClass = "btn-info btn btn-lg btn-home",
                    SpanClass = "fa fa-3x fa-user-md"
                });

            configuracoes.Add(new ConfiguracaoAtalho
            {
                Descricao = "Meu Perfil",
                BtnClass = "btn-success btn btn-lg btn-home",
                SpanClass = "fa fa-3x fa-user-circle-o"
            });

            configuracoes.Add(new ConfiguracaoAtalho
            {
                Descricao = "Conta a Pagar",
                Imagem = "../../../assets/images/icon/contaPagar.png",
                BtnClass = "btn-danger btn btn-lg btn-home"
            });

            configuracoes.Add(new ConfiguracaoAtalho
            {
                Descricao = "Conta a Receber",
                Imagem = "../../../assets/images/icon/contaReceber.png",
                BtnClass = "btn-warning btn btn-lg btn-home"
            });

            if (medico != null || (funcionario != null && funcionario.PermissaoAdministrador))
                configuracoes.Add(new ConfiguracaoAtalho
                {
                    Descricao = "Caixas",
                    Imagem = "../../../assets/images/icon/caixa.png",
                    BtnClass = "btn-primary btn btn-lg btn-home"
                });

            configuracoes.Add(new ConfiguracaoAtalho
            {
                Descricao = "Abrir Caixa",
                Imagem = "../../../assets/images/icon/abrir-caixa.png",
                BtnClass = "btn-success btn btn-lg btn-home"
            });

            configuracoes.Add(new ConfiguracaoAtalho
            {
                Descricao = "Fechar Caixa",
                Imagem = "../../../assets/images/icon/fechar-caixa.png",
                BtnClass = "btn-warning btn btn-lg btn-home"
            });

            configuracoes.Add(new ConfiguracaoAtalho
            {
                Descricao = "Configurar Atalhos",
                BtnClass = "btn-info btn btn-lg btn-home",
                SpanClass = "fa fa-3x fa-cog fa-spin"
            });

            return configuracoes;
        }

        public bool RemoveOne(string id)
        {
            return ConfiguracaoAtalhoNegocio.RemoveOne(ContextoConfiguracaoAtalhos.Collection, id);
        }
    }
}