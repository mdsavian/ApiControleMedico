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
    public class CirurgiaController : Controller
    {   
        private readonly CirurgiaService _cirurgiaService;

        public CirurgiaController(CirurgiaService cirurgiaService)
        {
            _cirurgiaService = cirurgiaService;
        }

        [HttpGet]
        public List<Cirurgia> Get()
        {
            var cirurgias = _cirurgiaService.GetAll();
            return cirurgias.ToList();
        }

        [HttpPost]
        public ActionResult<Cirurgia> Salvar(Cirurgia cirurgia)
        {
            var cirurgiaRetorno = _cirurgiaService.SaveOne(cirurgia);
            return cirurgiaRetorno;
        }

        [HttpGet, Route("buscarPorId/{cirurgiaId}")]
        public ActionResult<Cirurgia> BuscarPorId(string cirurgiaId)
        {
            return _cirurgiaService.GetOne(cirurgiaId);
        }

        [HttpDelete, Route("excluirPorId/{cirurgiaId}")]
        public ActionResult<bool> ExcluirPorId(string cirurgiaId)
        {
            return _cirurgiaService.RemoveOne(cirurgiaId);
        }
    }
}
