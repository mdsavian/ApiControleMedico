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
    public class ProcedimentoController : Controller
    {   
        private readonly ProcedimentoService _procedimentoService;

        public ProcedimentoController(ProcedimentoService procedimentoService)
        {
            _procedimentoService = procedimentoService;
        }

        [HttpGet]
        public List<Procedimento> Get()
        {
            var procedimentos = _procedimentoService.GetAllAsync();
            return procedimentos.Result.ToList();
        }

        [HttpPost]
        public ActionResult<Procedimento> Salvar(Procedimento procedimento)
        {
            var procedimentoRetorno = _procedimentoService.SaveOneAsync(procedimento);
            return procedimentoRetorno.Result;
        }

        [HttpGet, Route("buscarPorId/{procedimentoId}")]
        public ActionResult<Procedimento> BuscarPorId(string procedimentoId)
        {
            return _procedimentoService.GetOneAsync(procedimentoId).Result;
        }

        [HttpDelete, Route("excluirPorId/{procedimentoId}")]
        public ActionResult<bool> ExcluirPorId(string procedimentoId)
        {
            return _procedimentoService.RemoveOneAsync(procedimentoId).Result;
        }
    }
}
