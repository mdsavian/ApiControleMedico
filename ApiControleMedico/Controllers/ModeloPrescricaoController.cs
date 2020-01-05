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
    public class ModeloPrescricaoController : Controller
    {   
        private readonly ModeloPrescricaoService _modeloPrescricaoService;

        public ModeloPrescricaoController(ModeloPrescricaoService modeloPrescricaoService)
        {
            _modeloPrescricaoService = modeloPrescricaoService;
        }

        [HttpGet]
        public List<ModeloPrescricao> Get()
        {
            var modeloPrescricaos = _modeloPrescricaoService.GetAll();
            return modeloPrescricaos.ToList();
        }

        [HttpPost]
        public ActionResult<ModeloPrescricao> Salvar(ModeloPrescricao modeloPrescricao)
        {
            var modeloPrescricaoRetorno = _modeloPrescricaoService.SaveOne(modeloPrescricao);
            return modeloPrescricaoRetorno;
        }

        [HttpGet, Route("buscarPorId/{modeloPrescricaoId}")]
        public ActionResult<ModeloPrescricao> BuscarPorId(string modeloPrescricaoId)
        {
            return _modeloPrescricaoService.GetOne(modeloPrescricaoId);
        }

        [HttpDelete, Route("excluirPorId/{modeloPrescricaoId}")]
        public ActionResult<bool> ExcluirPorId(string modeloPrescricaoId)
        {
            return _modeloPrescricaoService.RemoveOne(modeloPrescricaoId);
        }
    }
}
