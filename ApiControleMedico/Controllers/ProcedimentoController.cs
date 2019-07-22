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
            var procedimentos = _procedimentoService.GetAll();
            return procedimentos.ToList();
        }

        [HttpPost]
        public ActionResult<Procedimento> Salvar(Procedimento procedimento)
        {
            var procedimentoRetorno = _procedimentoService.SaveOne(procedimento);
            return procedimentoRetorno;
        }

        [HttpGet, Route("buscarPorId/{procedimentoId}")]
        public ActionResult<Procedimento> BuscarPorId(string procedimentoId)
        {
            return _procedimentoService.GetOne(procedimentoId);
        }

        [HttpDelete, Route("excluirPorId/{procedimentoId}")]
        public ActionResult<bool> ExcluirPorId(string procedimentoId)
        {
            return _procedimentoService.RemoveOne(procedimentoId);
        }
    }
}
