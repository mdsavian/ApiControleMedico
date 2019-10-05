using System.Collections.Generic;
using System.Linq;
using ApiControleMedico.Modelos;
using ApiControleMedico.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiControleMedico.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ContaPagarController : Controller
    {
        private readonly ContaPagarService _contaPagarService;

        public ContaPagarController(ContaPagarService contaPagarService)
        {
            _contaPagarService = contaPagarService;
        }

        [HttpGet]
        public ActionResult<List<ContaPagar>> Get()
        {
            var lista = _contaPagarService.GetAll();
            return lista.ToList();
        }

        //[HttpGet, Route("TodosFiltrandoMedico/{medicoId}")]
        //public ActionResult<List<ContaPagar>> TodosFiltrandoMedico(string medicoId)
        //{
        //    var lista = _contaPagarService.TodosFiltrandoMedico(medicoId);
        //    return lista;
        //}

        [HttpGet, Route("buscarContaPagarPorFornecedor/{fornecedorId}")]
        public ActionResult<List<ContaPagar>> buscarContaPagarPorFornecedor(string fornecedorId)
        {
            var lista = _contaPagarService.buscarContaPagarPorFornecedor(fornecedorId);
            return lista;
        }

        [HttpGet, Route("buscarPorId/{contaPagarId}")]
        public ActionResult<ContaPagar> BuscarPorId(string contaPagarId)
        {
            return _contaPagarService.GetOne(contaPagarId);
        }

        [HttpPost]
        public ActionResult<ContaPagar> Salvar(ContaPagar contaPagar)
        {
            return _contaPagarService.SaveOne(contaPagar);
        }

        [HttpDelete, Route("excluirPorId/{contaPagarId}")]
        public ActionResult<bool> ExcluirPorId(string contaPagarId)
        {
            return _contaPagarService.RemoveOne(contaPagarId);
        }
    }
}
