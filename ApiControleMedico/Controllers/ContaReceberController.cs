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
    public class ContaReceberController : Controller
    {
        private readonly ContaReceberService _contaReceberService;

        public ContaReceberController(ContaReceberService contaReceberService)
        {
            _contaReceberService = contaReceberService;
        }

        [HttpGet, Route("Todos")]
        public List<ContaReceber> Todos([FromQuery] string usuarioId, [FromQuery] string clinicaId)
        {
            var medicos = new MedicoService().BuscarMedicosPorUsuario(usuarioId, clinicaId, false).Select(c => c.Id);
            var lista = _contaReceberService.GetAll().Where(c => c.ClinicaId == clinicaId && (c.MedicoId == "" || medicos.Contains(c.MedicoId)));
            return lista.ToList();
        }

        [HttpGet, Route("buscarPorId/{contaReceberId}")]
        public ActionResult<ContaReceber> BuscarPorId(string contaReceberId)
        {
            return _contaReceberService.GetOne(contaReceberId);
        }

        [HttpGet, Route("buscarPorAgendamento/{agendamentoId}")]
        public ActionResult<ContaReceber> BuscarPorAgendamento(string agendamentoId)
        {
            return _contaReceberService.BuscarPorAgendamento(agendamentoId);
        }

        [HttpPost]
        public ActionResult<ContaReceber> Salvar(ContaReceber contaReceber)
        {
            return _contaReceberService.SaveOne(contaReceber);
        }

        [HttpDelete, Route("excluirPorId/{contaReceberId}")]
        public ActionResult<bool> ExcluirPorId(string contaReceberId)
        {
            return _contaReceberService.RemoveOne(contaReceberId);
        }

        [HttpGet, Route("TodosPorPeriodo")]
        public List<ContaReceber> TodosPorPeriodo([FromQuery] string primeiroDiaMes, [FromQuery] string dataHoje, [FromQuery] string medicoId, [FromQuery] string funcionarioId, [FromQuery] string clinicaId)
        {
            return _contaReceberService.TodosPorPeriodo(primeiroDiaMes.ToDateTime(), dataHoje.ToDateTime(), medicoId, funcionarioId,clinicaId);

        }
    }
}
