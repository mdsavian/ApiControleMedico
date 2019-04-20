using System.Collections.Generic;
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

        public async Task<IEnumerable<ConfiguracaoAgenda>> GetAllAsync()
        {
            var configuracaoAgendas = await ConfiguracaoAgendaNegocio.GetAllAsync(ContextoConfiguracaoAgenda.Collection);
            return configuracaoAgendas;
        }

        public Task<ConfiguracaoAgenda> GetOneAsync(string id)
        {
            return ConfiguracaoAgendaNegocio.GetOneAsync(ContextoConfiguracaoAgenda.Collection, id);
        }

        public async Task<ConfiguracaoAgenda> SaveOneAsync(ConfiguracaoAgenda configuracaoAgenda)
        {
            await ConfiguracaoAgendaNegocio.SaveOneAsync(ContextoConfiguracaoAgenda.Collection, configuracaoAgenda);
            return configuracaoAgenda;
        }


        public Task<bool> RemoveOneAsync(string id)
        {
            return ConfiguracaoAgendaNegocio.RemoveOneAsync(ContextoConfiguracaoAgenda.Collection, id);
        }
    }
}