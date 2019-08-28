using System;
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
    public class AgendamentoPagamentoController : Controller
    {
        private readonly AgendamentoPagamentoService _agendamentoPagamentoService;

        public AgendamentoPagamentoController(AgendamentoPagamentoService agendamentoPagamentoService)
        {
            _agendamentoPagamentoService = agendamentoPagamentoService;
        }

        [HttpGet]
        public List<AgendamentoPagamento> Get()
        {
            var agendamentoPagamentos = _agendamentoPagamentoService.GetAll();
            return agendamentoPagamentos.ToList();
        }

        [HttpPost]
        public ActionResult<AgendamentoPagamento> Salvar(AgendamentoPagamento agendamentoPagamento)
        {
            var agendamentoPagamentoRetorno = _agendamentoPagamentoService.SaveOne(agendamentoPagamento);
            return agendamentoPagamentoRetorno;
        }

        [HttpGet, Route("buscarPorId/{agendamentoPagamentoId}")]
        public ActionResult<AgendamentoPagamento> BuscarPorId(string agendamentoPagamentoId)
        {
            return _agendamentoPagamentoService.GetOne(agendamentoPagamentoId);
        }

        [HttpDelete, Route("excluirPorId/{agendamentoPagamentoId}")]
        public ActionResult<bool> ExcluirPorId(string agendamentoPagamentoId)
        {
            return _agendamentoPagamentoService.RemoveOne(agendamentoPagamentoId);
        }
    }
}
