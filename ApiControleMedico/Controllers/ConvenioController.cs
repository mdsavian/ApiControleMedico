using System.Collections.Generic;
using System.Linq;
using ApiControleMedico.Modelos;
using ApiControleMedico.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiControleMedico.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ConvenioController : Controller
    {
        private readonly ConvenioService _convenioService;

        public ConvenioController(ConvenioService convenioService)
        {
            _convenioService = convenioService;
        }

        [HttpGet]
        public ActionResult<List<Convenio>> Get()
        {
            var lista = _convenioService.GetAllAsync();
            return lista.Result.ToList();
        }

        [HttpGet, Route("TodosFiltrandoMedico/{medicoId}")]
        public ActionResult<List<Convenio>> TodosFiltrandoMedico(string medicoId)
        {
            var lista = _convenioService.TodosFiltrandoMedico(medicoId);
            return lista;
        }

        [HttpPost]
        public ActionResult<Convenio> Salvar(Convenio convenio)
        {
            return _convenioService.SaveOneAsync(convenio).Result;
        }
    }
}
