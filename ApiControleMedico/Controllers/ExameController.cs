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
    public class ExameController : Controller
    {   
        private readonly ExameService _exameService;

        public ExameController(ExameService exameService)
        {
            _exameService = exameService;
        }

        [HttpGet]
        public List<Exame> Get()
        {
            var exames = _exameService.GetAll();
            return exames.ToList();
        }

        [HttpPost]
        public ActionResult<Exame> Salvar(Exame exame)
        {
            var exameRetorno = _exameService.SaveOne(exame);
            return exameRetorno;
        }

        [HttpGet, Route("buscarPorId/{exameId}")]
        public ActionResult<Exame> BuscarPorId(string exameId)
        {
            return _exameService.GetOne(exameId);
        }

        [HttpDelete, Route("excluirPorId/{exameId}")]
        public ActionResult<bool> ExcluirPorId(string exameId)
        {
            return _exameService.RemoveOne(exameId);
        }
    }
}
