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
        { return _agendamentoService.GetOne(agendamentoId); }

        [HttpDelete, Route("excluirPorId/{agendamentoId}")]
        public ActionResult<bool> ExcluirPorId(string agendamentoId)
        { return _agendamentoService.RemoveOne(agendamentoId); }

        [HttpGet, Route("buscarAgendamentosMedico")]
        public List<Agendamento> BuscarAgendamentosMedico([FromQuery]string medicoId, [FromQuery] string data, [FromQuery] string tipoCalendario, [FromQuery] string clinicaId)
        { return _agendamentoService.BuscarAgendamentosMedico(medicoId, data, tipoCalendario, clinicaId); }

        [HttpGet, Route("buscarAgendamentosPaciente")]
        public List<Agendamento> BuscarAgendamentosPaciente([FromQuery] string pacienteId, [FromQuery] string usuarioId, [FromQuery] string clinicaId)
        {
            return _agendamentoService.BuscarAgendamentosPaciente(pacienteId, usuarioId, clinicaId);
        }

        [HttpGet, Route("buscarAgendamentosFuncionario/{funcionarioId}")]
        public List<Agendamento> BuscarAgendamentosFuncionario(string funcionarioId)
        { return _agendamentoService.BuscarAgendamentosFuncionario(funcionarioId); }

        [HttpGet, Route("buscarAgendamentosCaixa")]
        public List<Agendamento> BuscarAgendamentosCaixa([FromQuery] string caixaId, [FromQuery] string clinicaId)
        { return _agendamentoService.BuscarAgendamentosCaixa(caixaId, clinicaId); }

        [HttpGet, Route("buscarAgendamentosCirurgia/{cirurgiaId}")]
        public List<Agendamento> BuscarAgendamentosCirurgia(string cirurgiaId)
        { return _agendamentoService.BuscarAgendamentosCirurgia(cirurgiaId); }

        [HttpGet, Route("buscarAgendamentosConvenio/{convenioId}")]
        public List<Agendamento> BuscarAgendamentosConvenio(string convenioId)
        { return _agendamentoService.BuscarAgendamentosConvenio(convenioId); }

        [HttpGet, Route("buscarAgendamentosExame/{exameId}")]
        public List<Agendamento> BuscarAgendamentosExame(string exameId)
        { return _agendamentoService.BuscarAgendamentosExame(exameId); }

        [HttpGet, Route("buscarAgendamentosProcedimento/{procedimentoId}")]
        public List<Agendamento> BuscarAgendamentosProcedimento(string procedimentoId)
        { return _agendamentoService.BuscarAgendamentosProcedimento(procedimentoId); }

        [HttpGet, Route("buscarAgendamentosLocal/{localId}")]
        public List<Agendamento> BuscarAgendamentosLocal(string localId)
        { return _agendamentoService.BuscarAgendamentosLocal(localId); }

        [HttpGet, Route("buscarPagamentoAgendamentoForma/{formaPagamentoId}")]
        public List<Agendamento> BuscarPagamentoAgendamentoForma(string formaPagamentoId)
        { return _agendamentoService.BuscarPagamentoAgendamentoForma(formaPagamentoId); }

        [HttpGet, Route("BuscarAgendamentoMedicoExcluir/{medicoId}")]
        public List<Agendamento> BuscarAgendamentoMedicoExcluir(string medicoId)
        { return _agendamentoService.BuscarAgendamentoMedicoExcluir(medicoId); }

        [HttpGet, Route("BuscarUltimoAgendamentoPaciente")]
        public Agendamento BuscarUltimoAgendamentoPaciente([FromQuery]string pacienteId, [FromQuery] string agendamentoId)
        {
            return _agendamentoService.BuscarUltimoAgendamentoPaciente(pacienteId, agendamentoId);
        }

        [HttpGet, Route("ProcedimentosRealizados")]
        public List<Agendamento> ProcedimentosRealizados([FromQuery]string dataInicio, [FromQuery] string dataFim, [FromQuery] string medicoId)
        {
            return _agendamentoService.ProcedimentosRealizados(dataInicio.ToDateTime(),dataFim.ToDateTime(), medicoId);
        }


        [HttpGet, Route("TodosPorPeriodo")]
        public List<Agendamento> TodosPorPeriodo([FromQuery]string primeiroDiaMes, [FromQuery] string dataHoje, [FromQuery] string medicoId, [FromQuery] string caixaId, [FromQuery] string funcionarioId, [FromQuery] string clinicaId)
        { return _agendamentoService.TodosPorPeriodo(primeiroDiaMes.ToDateTime(), dataHoje.ToDateTime(), medicoId, caixaId, funcionarioId,clinicaId); }
    }
}
