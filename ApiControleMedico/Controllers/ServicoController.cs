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
    public class ServicoController : Controller
    {   
        private readonly ServicoService _servicoService;

        public ServicoController(ServicoService servicoService)
        {
            _servicoService = servicoService;
        }

        [HttpGet]
        public List<Servico> Get()
        {
            var servicos = _servicoService.GetAllAsync();
            return servicos.Result.ToList();
        }

        [HttpPost]
        public ActionResult<Servico> Salvar(Servico servico)
        {
            var servicoRetorno = _servicoService.SaveOneAsync(servico);
            return servicoRetorno.Result;
        }

        [HttpGet, Route("buscarPorId/{servicoId}")]
        public ActionResult<Servico> BuscarPorId(string servicoId)
        {
            return _servicoService.GetOneAsync(servicoId).Result;
        }

        [HttpDelete, Route("excluirPorId/{servicoId}")]
        public ActionResult<bool> ExcluirPorId(string servicoId)
        {
            return _servicoService.RemoveOneAsync(servicoId).Result;
        }
    }
}
