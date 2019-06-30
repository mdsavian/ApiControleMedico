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
    public class ClinicaController : Controller
    {   
        private readonly ClinicaService _clinicaService;

        public ClinicaController(ClinicaService clinicaService)
        {
            _clinicaService = clinicaService;
        }

        [HttpGet]
        public List<Clinica> Get()
        {
            var clinicas = _clinicaService.GetAllAsync();
            return clinicas.Result.ToList();
        }

        [HttpPost]
        public ActionResult<Clinica> Salvar(Clinica clinica)
        {
            var clinicaRetorno = _clinicaService.SaveOneAsync(clinica);
            return clinicaRetorno.Result;
        }

        [HttpGet, Route("buscarPorId/{clinicaId}")]
        public ActionResult<Clinica> BuscarPorId(string clinicaId)
        {
            return _clinicaService.GetOneAsync(clinicaId).Result;
        }

        [HttpDelete, Route("excluirPorId/{clinicaId}")]
        public ActionResult<bool> ExcluirPorId(string clinicaId)
        {
            return _clinicaService.RemoveOneAsync(clinicaId).Result;
        }
    }
}
