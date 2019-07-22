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
    public class OficioController : Controller
    {   
        private readonly OficioService _oficioService;

        public OficioController(OficioService oficioService)
        {
            _oficioService = oficioService;
        }

        [HttpGet]
        public List<Oficio> Get()
        {
            var oficios = _oficioService.GetAll();
            return oficios.ToList();
        }

        [HttpPost]
        public ActionResult<Oficio> Salvar(Oficio oficio)
        {
            var oficioRetorno = _oficioService.SaveOne(oficio);
            return oficioRetorno;
        }

        [HttpGet, Route("buscarPorId/{oficioId}")]
        public ActionResult<Oficio> BuscarPorId(string oficioId)
        {
            return _oficioService.GetOne(oficioId);
        }

        [HttpDelete, Route("excluirPorId/{oficioId}")]
        public ActionResult<bool> ExcluirPorId(string oficioId)
        {
            return _oficioService.RemoveOne(oficioId);
        }
    }
}
