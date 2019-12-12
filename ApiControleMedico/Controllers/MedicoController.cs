using System.Collections.Generic;
using System.Linq;
using ApiControleMedico.Modelos;
using ApiControleMedico.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiControleMedico.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class MedicoController : Controller
    {
        private readonly MedicoService _medicoService;

        public MedicoController(MedicoService medicoService)
        {
            _medicoService = medicoService;
        }

        [HttpGet]
        public List<Medico> Get()
        {
            var medicos = _medicoService.GetAll();
            return medicos.ToList();
        }

        [HttpPost]
        public ActionResult<Medico> Salvar(Medico medico)
        {
            var medicoRetorno =  _medicoService.SaveOne(medico);
            return medicoRetorno;
        }

        //[HttpPost, Route("salvarConfiguracaoAgendaMedico")]
        //public ActionResult<Medico> SalvarConfiguracaoMedico(Medico medico)
        //{
        //    var medicoRetorno = _medicoService.SalvarConfiguracaoMedico(medico);
        //    return medicoRetorno;
        //}

        [HttpGet, Route("todos")]
        public List<Medico> Todos([FromQuery] bool carregarEspecialidade)
        {
            return _medicoService.Todos(carregarEspecialidade);
        }

        [HttpGet, Route("buscarPorId/{medicoId}")]
        public ActionResult<Medico> BuscarPorId(string medicoId)
        {
            return _medicoService.GetOne(medicoId);
        }

        [HttpGet, Route("buscarConfiguracaoAgenda/")]
        public ActionResult<ConfiguracaoAgenda> BuscarConfiguracaoAgendaMedico([FromQuery]string medicoId, [FromQuery] string clinicaId)
        {
            return _medicoService.BuscarConfiguracaoAgenda(medicoId, clinicaId);
        }

        [HttpGet, Route("todosFiltrandoMedico/{medicoId}")]
        public ActionResult<List<Medico>> TodosFiltrandoMedico(string medicoId)
        {
            return _medicoService.TodosFiltrandoMedico(medicoId);
        }

        [HttpGet, Route("validarDeleteConvenioMedico/")]
        public ActionResult<bool> ValidarDeleteConvenioMedico([FromQuery]string medicoId, [FromQuery] string convenioId)
        {
            return _medicoService.ValidarDeleteConvenioMedico(medicoId, convenioId);
        }

        [HttpGet, Route("buscarMedicosPorUsuario/")]
        public ActionResult<List<Medico>> BuscarMedicosPorUsuario([FromQuery]string usuarioId, [FromQuery] string clinicaId, [FromQuery]bool carregarEspecialidade)
        {
            return _medicoService.BuscarMedicosPorUsuario(usuarioId, clinicaId, carregarEspecialidade);
        }

        [HttpDelete, Route("excluirPorId/{medicoId}")]
        public ActionResult<bool> ExcluirPorId(string medicoId
        )
        {
            return _medicoService.RemoveOne(medicoId);
        }

        [HttpGet, Route("BuscarMedicoConvenio/{convenioId}")]
        public List<Medico> BuscarMedicoConvenio(string convenioId)
        {
            return _medicoService.BuscarMedicoConvenio(convenioId);
        }

        [HttpGet, Route("BuscarMedicoEspecialidade/{especialidadeId}")]
        public List<Medico> BuscarMedicoEspecialidade(string especialidadeId)
        {
            return _medicoService.BuscarMedicoEspecialidade(especialidadeId);
        }
    }
}
