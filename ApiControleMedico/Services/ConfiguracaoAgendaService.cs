using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiControleMedico.Modelos;
using ApiControleMedico.Repositorio;

namespace ApiControleMedico.Services
{
    public class ConfiguracaoAgendaService
    {
        protected readonly DbContexto<ConfiguracaoAgenda> ContextoConfiguracaoAgenda;
        protected readonly EntidadeNegocio<ConfiguracaoAgenda> ConfiguracaoAgendaNegocio = new EntidadeNegocio<ConfiguracaoAgenda>();

        public ConfiguracaoAgendaService()
        {
            ContextoConfiguracaoAgenda = new DbContexto<ConfiguracaoAgenda>("configuracaoAgenda");
        }

        public IEnumerable<ConfiguracaoAgenda> GetAll()
        {
            return ConfiguracaoAgendaNegocio.GetAll(ContextoConfiguracaoAgenda.Collection).ToList();
        }

        public ConfiguracaoAgenda GetOne(string id)
        {
            return ConfiguracaoAgendaNegocio.GetOne(ContextoConfiguracaoAgenda.Collection, id);
        }

        public ConfiguracaoAgenda SaveOne(ConfiguracaoAgenda configuracaoAgenda)
        {
            ConfiguracaoAgendaNegocio.SaveOne(ContextoConfiguracaoAgenda.Collection, configuracaoAgenda);
            return configuracaoAgenda;
        }


        public bool RemoveOne(string id)
        {
            return ConfiguracaoAgendaNegocio.RemoveOne(ContextoConfiguracaoAgenda.Collection, id);
        }
    }
}