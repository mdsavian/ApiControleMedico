using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiControleMedico.Modelos;
using ApiControleMedico.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiControleMedico.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ConfiguracaoAtalhoController : Controller
    {   
        private readonly ConfiguracaoAtalhoService _configuracaoAtalhoService;

        public ConfiguracaoAtalhoController(ConfiguracaoAtalhoService configuracaoAtalhoService)
        {
            _configuracaoAtalhoService = configuracaoAtalhoService;
        }

        [HttpGet]
        public List<ConfiguracaoAtalho> Get()
        {
            var configuracaoAtalhos = _configuracaoAtalhoService.GetAll();
            return configuracaoAtalhos.ToList();
        }

        [HttpPost]
        public ActionResult<ConfiguracaoAtalho> Salvar(ConfiguracaoAtalho configuracaoAtalho)
        {
            var configuracaoAtalhoRetorno = _configuracaoAtalhoService.SaveOne(configuracaoAtalho);
            return configuracaoAtalhoRetorno;
        }

        [HttpGet, Route("buscarPorId/{configuracaoAtalhoId}")]
        public ActionResult<ConfiguracaoAtalho> BuscarPorId(string configuracaoAtalhoId)
        {
            return _configuracaoAtalhoService.GetOne(configuracaoAtalhoId);
        }

        [HttpGet, Route("BuscarPorUsuario/{usuarioId}")]
        public ActionResult<List<ConfiguracaoAtalho>> BuscarPorUsuario(string usuarioId)
        {
            return _configuracaoAtalhoService.BuscarPorUsuario(usuarioId);
        }

        [HttpGet, Route("buscarParaConfiguracao/{usuarioId}")]
        public ActionResult<List<ConfiguracaoAtalho>> BuscarParaConfiguracao(string usuarioId)
        {
            return _configuracaoAtalhoService.BuscarParaConfiguracao(usuarioId);
        }

        [HttpDelete, Route("excluirPorId/{configuracaoAtalhoId}")]
        public ActionResult<bool> ExcluirPorId(string configuracaoAtalhoId)
        {
            return _configuracaoAtalhoService.RemoveOne(configuracaoAtalhoId);
        }
    }
}
