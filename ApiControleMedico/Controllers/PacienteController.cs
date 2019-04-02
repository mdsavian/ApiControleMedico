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
    public class PacienteController : Controller
    {
        private readonly PacienteService _pacienteService;

        public PacienteController(PacienteService pacienteService)
        {
            _pacienteService = pacienteService;
        }

        [HttpGet]
        public List<Paciente> Get()
        {
            var pacientes = _pacienteService.GetAllAsync();
            return pacientes.Result.ToList();
        }

        [HttpPost]
        public ActionResult<Paciente> Salvar(Paciente paciente)
        {
            var pacienteRetorno = _pacienteService.SaveOneAsync(paciente);
            return pacienteRetorno.Result;
        }

        [HttpGet, Route("buscarPorId/{pacienteId}")]
        public ActionResult<Paciente> BuscarPorId(string pacienteId)
        {
            return _pacienteService.GetOneAsync(pacienteId).Result;
        }

        [HttpGet, Route("TodosGestantesFiltrandoMedico/{medicoId}")]
        public ActionResult<List<Paciente>> TodosGestantesFiltrandoMedico(string medicoId)
        {
            return _pacienteService.TodosGestantesFiltrandoMedico(medicoId);
        }

        [HttpDelete, Route("excluirPorId/{pacienteId}")]
        public ActionResult<bool> ExcluirPorId(string pacienteId)
        {
            return _pacienteService.RemoveOneAsync(pacienteId).Result;
        }
    }
}
