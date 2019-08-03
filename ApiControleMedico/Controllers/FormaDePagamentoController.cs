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
    public class FormaDePagamentoController : Controller
    {   
        private readonly FormaDePagamentoService _formaDePagamentoService;

        public FormaDePagamentoController(FormaDePagamentoService formaDePagamentoService)
        {
            _formaDePagamentoService = formaDePagamentoService;
        }

        [HttpGet]
        public List<FormaDePagamento> Get()
        {
            var formaDePagamentos = _formaDePagamentoService.GetAll();
            return formaDePagamentos.ToList();
        }

        [HttpPost]
        public ActionResult<FormaDePagamento> Salvar(FormaDePagamento formaDePagamento)
        {
            var formaDePagamentoRetorno = _formaDePagamentoService.SaveOne(formaDePagamento);
            return formaDePagamentoRetorno;
        }

        [HttpGet, Route("buscarPorId/{formaDePagamentoId}")]
        public ActionResult<FormaDePagamento> BuscarPorId(string formaDePagamentoId)
        {
            return _formaDePagamentoService.GetOne(formaDePagamentoId);
        }

        [HttpDelete, Route("excluirPorId/{formaDePagamentoId}")]
        public ActionResult<bool> ExcluirPorId(string formaDePagamentoId)
        {
            return _formaDePagamentoService.RemoveOne(formaDePagamentoId);
        }
    }
}
