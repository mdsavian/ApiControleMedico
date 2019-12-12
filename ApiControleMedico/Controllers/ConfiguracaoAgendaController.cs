using System.Collections.Generic;
using System.Linq;
using ApiControleMedico.Modelos;
using ApiControleMedico.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiControleMedico.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ConfiguracaoAgendaController : Controller
    {   
        private readonly ConfiguracaoAgendaService _configuracaoAgendaService;

        public ConfiguracaoAgendaController(ConfiguracaoAgendaService configuracaoAgendaService)
        {
            _configuracaoAgendaService = configuracaoAgendaService;
        }

        [HttpGet]
        public List<ConfiguracaoAgenda> Get()
        {
            var configuracaoAgendas = _configuracaoAgendaService.GetAll();
            return configuracaoAgendas.ToList();
        }

        [HttpPost]
        public ActionResult<ConfiguracaoAgenda> Salvar(ConfiguracaoAgenda configuracaoAgenda)
        {
            var configuracaoAgendaRetorno = _configuracaoAgendaService.SaveOne(configuracaoAgenda);
            return configuracaoAgendaRetorno;
        }

        [HttpGet, Route("buscarPorId/{configuracaoAgendaId}")]
        public ActionResult<ConfiguracaoAgenda> BuscarPorId(string configuracaoAgendaId)
        {
            return _configuracaoAgendaService.GetOne(configuracaoAgendaId);
        }

        [HttpGet, Route("buscarConfiguracaoAgenda/")]
        public ActionResult<ConfiguracaoAgenda> BuscarConfiguracaoAgenda([FromQuery]string medicoId, [FromQuery]string clinicaId)
        {
            return _configuracaoAgendaService.BuscarConfiguracaoAgenda(medicoId, clinicaId);
        }

        
    }
}
