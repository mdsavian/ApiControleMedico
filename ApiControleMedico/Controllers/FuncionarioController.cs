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
    public class FuncionarioController : Controller
    {   
        private readonly FuncionarioService _funcionarioService;

        public FuncionarioController(FuncionarioService funcionarioService)
        {
            _funcionarioService = funcionarioService;
        }

        [HttpGet]
        public List<Funcionario> Get()
        {
            var funcionarios = _funcionarioService.GetAllAsync();
            return funcionarios.Result.ToList();
        }

        [HttpPost]
        public ActionResult<Funcionario> Salvar(Funcionario funcionario)
        {
            var funcionarioRetorno = _funcionarioService.SaveOneAsync(funcionario);
            return funcionarioRetorno.Result;
        }

        [HttpGet, Route("buscarPorId/{funcionarioId}")]
        public ActionResult<Funcionario> BuscarPorId(string funcionarioId)
        {
            return _funcionarioService.GetOneAsync(funcionarioId).Result;
        }

        [HttpDelete, Route("excluirPorId/{funcionarioId}")]
        public ActionResult<bool> ExcluirPorId(string funcionarioId)
        {
            return _funcionarioService.RemoveOneAsync(funcionarioId).Result;
        }
    }
}
