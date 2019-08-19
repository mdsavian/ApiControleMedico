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
    public class CaixaController : Controller
    {   
        private readonly CaixaService _caixaService;

        public CaixaController(CaixaService caixaService)
        {
            _caixaService = caixaService;
        }

        [HttpGet]
        public List<Caixa> Get()
        {
            var caixas = _caixaService.GetAll();
            return caixas.ToList();
        }

        [HttpPost]
        public ActionResult<Caixa> Salvar(Caixa caixa)
        {
            var caixaRetorno = _caixaService.SaveOne(caixa);
            return caixaRetorno;
        }

        [HttpGet, Route("buscarPorId/{caixaId}")]
        public ActionResult<Caixa> BuscarPorId(string caixaId)
        {
            return _caixaService.GetOne(caixaId);
        }

        [HttpGet, Route("validarCaixaAbertoFuncionario/{funcionarioId}")]
        public ActionResult<bool> ValidarCaixaAbertoFuncionario(string funcionarioId)
        {
            return _caixaService.ValidarCaixaAbertoFuncionario(funcionarioId);
        }

        [HttpDelete, Route("excluirPorId/{caixaId}")]
        public ActionResult<bool> ExcluirPorId(string caixaId)
        {
            return _caixaService.RemoveOne(caixaId);
        }
    }
}
