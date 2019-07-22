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
            var lista = _convenioService.GetAll();
            return lista.ToList();
        }

        [HttpGet, Route("TodosFiltrandoMedico/{medicoId}")]
        public ActionResult<List<Convenio>> TodosFiltrandoMedico(string medicoId)
        {
            var lista = _convenioService.TodosFiltrandoMedico(medicoId);
            return lista;
        }

        [HttpGet, Route("buscarMedicosPorConvenio/{convenioId}")]
        public ActionResult<List<Medico>> BuscarMedicosPorConvenio(string convenioId)
        {
            var lista = _convenioService.BuscarMedicosPorConvenio(convenioId);
            return lista;
        }

        [HttpGet, Route("buscarPorId/{convenioId}")]
        public ActionResult<Convenio> BuscarPorId(string convenioId)
        {
            return _convenioService.GetOne(convenioId);
        }

        [HttpPost]
        public ActionResult<Convenio> Salvar(Convenio convenio)
        {
            return _convenioService.SaveOne(convenio);
        }

        [HttpDelete, Route("excluirPorId/{convenioId}")]
        public ActionResult<bool> ExcluirPorId(string convenioId)
        {
            return _convenioService.RemoveOne(convenioId);
        }
    }
}
