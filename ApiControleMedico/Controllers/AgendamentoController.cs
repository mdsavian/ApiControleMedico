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
    public class AgendamentoController : Controller
    {
        private readonly AgendamentoService _agendamentoService;

        public AgendamentoController(AgendamentoService agendamentoService)
        {
            _agendamentoService = agendamentoService;
        }

        [HttpGet]
        public List<Agendamento> Get()
        {
            var agendamentos = _agendamentoService.GetAll();
            return agendamentos.ToList();
        }

        [HttpPost]
        public ActionResult<Agendamento> Salvar(Agendamento agendamento)
        {
            var agendamentoRetorno = _agendamentoService.SaveOne(agendamento);
            return agendamentoRetorno;
        }

        [HttpGet, Route("buscarPorId/{agendamentoId}")]
        public ActionResult<Agendamento> BuscarPorId(string agendamentoId)
        {
            return _agendamentoService.GetOne(agendamentoId);
        }

        [HttpDelete, Route("excluirPorId/{agendamentoId}")]
        public ActionResult<bool> ExcluirPorId(string agendamentoId)
        {
            return _agendamentoService.RemoveOne(agendamentoId);
        }

        [HttpGet, Route("buscarAgendamentosMedico")]
        public List<Agendamento> BuscarAgendamentosMedico([FromQuery]string medicoId, [FromQuery] string data, [FromQuery] string tipoCalendario)
        {
            return _agendamentoService.BuscarAgendamentoMedico(medicoId, data, tipoCalendario);
        }
    }
}
