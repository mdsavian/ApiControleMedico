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
            var agendamentos = _agendamentoService.GetAllAsync();
            return agendamentos.Result.ToList();
        }

        [HttpPost]
        public ActionResult<Agendamento> Salvar(Agendamento agendamento)
        {
            var agendamentoRetorno = _agendamentoService.SaveOneAsync(agendamento);
            return agendamentoRetorno.Result;
        }

        [HttpGet, Route("buscarPorId/{agendamentoId}")]
        public ActionResult<Agendamento> BuscarPorId(string agendamentoId)
        {
            return _agendamentoService.GetOneAsync(agendamentoId).Result;
        }

        [HttpDelete, Route("excluirPorId/{agendamentoId}")]
        public ActionResult<bool> ExcluirPorId(string agendamentoId)
        {
            return _agendamentoService.RemoveOneAsync(agendamentoId).Result;
        }

        [HttpGet, Route("buscarAgendamentosMedico")]
        public List<Agendamento> BuscarAgendamentosMedico([FromQuery]string medicoId, [FromQuery] string data, [FromQuery] string tipoCalendario)
        {
            return _agendamentoService.BuscarAgendamentoMedico(medicoId, data, tipoCalendario);
        }
    }
}
