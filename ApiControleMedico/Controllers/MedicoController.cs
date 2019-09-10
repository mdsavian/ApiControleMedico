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

        [HttpPost, Route("salvarConfiguracaoAgendaMedico")]
        public ActionResult<Medico> SalvarConfiguracaoMedico(Medico medico)
        {
            var medicoRetorno = _medicoService.SalvarConfiguracaoMedico(medico);
            return medicoRetorno;
        }

        [HttpGet, Route("buscarPorId/{medicoId}")]
        public ActionResult<Medico> BuscarPorId(string medicoId)
        {
            return _medicoService.GetOne(medicoId);
        }

        [HttpGet, Route("buscarConfiguracaoAgendaMedico/{configuracaoAgendaId}")]
        public ActionResult<ConfiguracaoAgenda> BuscarConfiguracaoAgendaMedico(string configuracaoAgendaId)
        {
            return _medicoService.BuscarConfiguracaoAgenda(configuracaoAgendaId);
        }

        [HttpGet, Route("todosFiltrandoMedico/{medicoId}")]
        public ActionResult<List<Medico>> TodosFiltrandoMedico(string medicoId)
        {
            return _medicoService.TodosFiltrandoMedico(medicoId);
        }


        [HttpDelete, Route("excluirPorId/{medicoId}")]
        public ActionResult<bool> ExcluirPorId(string medicoId
        )
        {
            return _medicoService.RemoveOne(medicoId);
        }

        [HttpPost, Route("buscarMedicoUsuario")]
        public ActionResult<Medico> BuscarMedicoUsuario(Usuario usuario)
        {
            return _medicoService.BuscarMedicoUsuario(usuario);
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
