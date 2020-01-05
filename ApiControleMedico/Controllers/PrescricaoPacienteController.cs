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
    public class PrescricaoPacienteController : Controller
    {   
        private readonly PrescricaoPacienteService _prescricaoPacienteService;

        public PrescricaoPacienteController(PrescricaoPacienteService prescricaoPacienteService)
        {
            _prescricaoPacienteService = prescricaoPacienteService;
        }

        [HttpGet]
        public List<PrescricaoPaciente> Get()
        {
            var prescricaoPacientes = _prescricaoPacienteService.GetAll();
            return prescricaoPacientes.ToList();
        }

        [HttpPost]
        public ActionResult<PrescricaoPaciente> Salvar(PrescricaoPaciente prescricaoPaciente)
        {
            var prescricaoPacienteRetorno = _prescricaoPacienteService.SaveOne(prescricaoPaciente);
            return prescricaoPacienteRetorno;
        }

        [HttpGet, Route("buscarPorId/{prescricaoPacienteId}")]
        public ActionResult<PrescricaoPaciente> BuscarPorId(string prescricaoPacienteId)
        {
            return _prescricaoPacienteService.BuscarPorId(prescricaoPacienteId);
        }

        [HttpGet, Route("BuscarPorPaciente")]
        public ActionResult<List<PrescricaoPaciente>> BuscarPorPaciente([FromQuery] string pacienteId, [FromQuery] string usuarioId, [FromQuery]string clinicaId)
        {
            return _prescricaoPacienteService.BuscarPorPaciente(pacienteId, usuarioId,clinicaId);
        }

        [HttpDelete, Route("excluirPorId/{prescricaoPacienteId}")]
        public ActionResult<bool> ExcluirPorId(string prescricaoPacienteId)
        {
            return _prescricaoPacienteService.RemoveOne(prescricaoPacienteId);
        }
    }
}
