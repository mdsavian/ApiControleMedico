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
            var especialidades = _especialidadeService.GetAll();
            return especialidades.ToList();
        }

        [HttpPost]
        public ActionResult<Especialidade> Salvar(Especialidade especialidade)
        {
            var especialidadeRetorno = _especialidadeService.SaveOne(especialidade);
            return especialidadeRetorno;
        }

        [HttpGet, Route("buscarPorId/{especialidadeId}")]
        public ActionResult<Especialidade> BuscarPorId(string especialidadeId)
        {
            return _especialidadeService.GetOne(especialidadeId);
        }

        [HttpDelete, Route("excluirPorId/{especialidadeId}")]
        public ActionResult<bool> ExcluirPorId(string especialidadeId)
        {
            return _especialidadeService.RemoveOne(especialidadeId);
        }
    }
}
