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

        private async Task<List<Paciente>> BuscaAll()
        {
            var xx = await _pacienteService.GetAllAsync();

            return xx.ToList();
        }

        [HttpGet]
        public ActionResult<List<Paciente>> Get()
        {
            var xx = BuscaAll();
            xx.Wait();
            return xx.Result;
        }
    }
}
