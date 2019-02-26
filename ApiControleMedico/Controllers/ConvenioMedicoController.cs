using System.Collections.Generic;
using System.Linq;
using ApiControleMedico.Modelos;
using ApiControleMedico.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiControleMedico.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ConvenioMedicoController : Controller
    {
        private readonly ConvenioMedicoService _convenioMedicoService;

        public ConvenioMedicoController(ConvenioMedicoService convenioMedicoService)
        {
            _convenioMedicoService = convenioMedicoService;
        }

        [HttpGet]
        public ActionResult<List<ConvenioMedico>> Get()
        {
            var lista = _convenioMedicoService.GetAllAsync();
            return lista.Result.ToList();
        }

        [HttpPost]
        public ActionResult<ConvenioMedico> Salvar(ConvenioMedico convenio)
        {
            return _convenioMedicoService.SaveOneAsync(convenio).Result;
        }

        [Route("conveniodoMedico/{medicoId}")]
        public ActionResult<List<ConvenioMedico>> ConvenioDoMedico(string medicoId)
        {
            var xx = _convenioMedicoService.BuscarConvenioMedico(medicoId);
            return xx;
        }

        //[HttpPost]
        //public ActionResult<Usuario> Convenio(Usuario usuario)
        //{

        //}
    }
}
