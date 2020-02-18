using System;
using System.Collections.Generic;
using System.Linq;
using ApiControleMedico.Modelos;
using ApiControleMedico.Services;
using Microsoft.AspNetCore.Mvc;
using ApiControleMedico.Uteis;

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

        [HttpGet, Route("Todos")]
        public List<ContaPagar> Todos([FromQuery] string usuarioId, [FromQuery] string clinicaId)
        {
            var medicos = new MedicoService().BuscarMedicosPorUsuario(usuarioId, clinicaId, false).Select(c => c.Id);
            var lista = _contaPagarService.GetAll().Where(c => c.ClinicaId == clinicaId && (c.MedicoId == "" || medicos.Contains(c.MedicoId)));
            return lista.ToList();
        }

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

        [HttpGet, Route("TodosPorPeriodo")]
        public List<ContaPagar> TodosPorPeriodo([FromQuery]string primeiroDiaMes, [FromQuery] string dataHoje, [FromQuery] string medicoId, [FromQuery] string funcionarioId)
        { return _contaPagarService.TodosPorPeriodo(primeiroDiaMes.ToDateTime(), dataHoje.ToDateTime(), medicoId, funcionarioId); }
    }
}
