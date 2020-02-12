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
    public class ExtraCaixaController : Controller
    {   
        private readonly ExtraCaixaService _extraCaixaService;

        public ExtraCaixaController(ExtraCaixaService extraCaixaService)
        {
            _extraCaixaService = extraCaixaService;
        }

        [HttpGet]
        public List<ExtraCaixa> Get()
        {
            var extraCaixas = _extraCaixaService.GetAll();
            return extraCaixas.ToList();
        }

        [HttpPost]
        public ActionResult<ExtraCaixa> Salvar(ExtraCaixa extraCaixa)
        {
            var extraCaixaRetorno = _extraCaixaService.SaveOne(extraCaixa);
            return extraCaixaRetorno;
        }

        [HttpGet, Route("buscarPorId/{extraCaixaId}")]
        public ActionResult<ExtraCaixa> BuscarPorId(string extraCaixaId)
        {
            return _extraCaixaService.GetOne(extraCaixaId);
        }

        [HttpDelete, Route("excluirPorId/{extraCaixaId}")]
        public ActionResult<bool> ExcluirPorId(string extraCaixaId)
        {
            return _extraCaixaService.RemoveOne(extraCaixaId);
        }

        [HttpGet, Route("buscarPorCaixa/{caixaId}")]
        public List<ExtraCaixa> BuscarPorCaixa(string caixaId)
        {
            return _extraCaixaService.BuscarPorCaixa(caixaId);
        }
    }
}
