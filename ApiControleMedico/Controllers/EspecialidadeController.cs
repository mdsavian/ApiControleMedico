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
    public class EspecialidadeController : Controller
    {   
        private readonly EspecialidadeService _especialidadeService;

        public EspecialidadeController(EspecialidadeService especialidadeService)
        {
            _especialidadeService = especialidadeService;
        }

        [HttpGet]
        public List<Especialidade> Get()
        {
            var especialidades = _especialidadeService.GetAllAsync();
            return especialidades.Result.ToList();
        }

        [HttpPost]
        public ActionResult<Especialidade> Salvar(Especialidade especialidade)
        {
            var especialidadeRetorno = _especialidadeService.SaveOneAsync(especialidade);
            return especialidadeRetorno.Result;
        }

        [HttpGet, Route("buscarPorId/{especialidadeId}")]
        public ActionResult<Especialidade> BuscarPorId(string especialidadeId)
        {
            return _especialidadeService.GetOneAsync(especialidadeId).Result;
        }

        [HttpDelete, Route("excluirPorId/{especialidadeId}")]
        public ActionResult<bool> ExcluirPorId(string especialidadeId)
        {
            return _especialidadeService.RemoveOneAsync(especialidadeId).Result;
        }
    }
}
