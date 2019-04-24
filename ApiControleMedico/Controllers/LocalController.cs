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
    public class LocalController : Controller
    {   
        private readonly LocalService _localService;

        public LocalController(LocalService localService)
        {
            _localService = localService;
        }

        [HttpGet]
        public List<Local> Get()
        {
            var locals = _localService.GetAllAsync();
            return locals.Result.ToList();
        }

        [HttpPost]
        public ActionResult<Local> Salvar(Local local)
        {
            var localRetorno = _localService.SaveOneAsync(local);
            return localRetorno.Result;
        }

        [HttpGet, Route("buscarPorId/{localId}")]
        public ActionResult<Local> BuscarPorId(string localId)
        {
            return _localService.GetOneAsync(localId).Result;
        }

        [HttpDelete, Route("excluirPorId/{localId}")]
        public ActionResult<bool> ExcluirPorId(string localId)
        {
            return _localService.RemoveOneAsync(localId).Result;
        }
    }
}
