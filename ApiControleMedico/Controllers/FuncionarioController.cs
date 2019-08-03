using System.Collections.Generic;
using System.Linq;
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
            var funcionarios = _funcionarioService.GetAll();
            return funcionarios.ToList();
        }

        [HttpPost]
        public ActionResult<Funcionario> Salvar(Funcionario funcionario)
        {
            var funcionarioRetorno = _funcionarioService.SaveOne(funcionario);

            return funcionarioRetorno;
        }

        [HttpGet, Route("buscarPorId/{funcionarioId}")]
        public ActionResult<Funcionario> BuscarPorId(string funcionarioId)
        {
            return _funcionarioService.GetOne(funcionarioId);
        }


        [HttpGet, Route("buscarComMedicos/{funcionarioId}")]
        public ActionResult<Funcionario> BuscarComMedicos(string funcionarioId)
        {
            return _funcionarioService.BuscarComMedicos(funcionarioId);
        }


        [HttpDelete, Route("excluirPorId/{funcionarioId}")]
        public ActionResult<bool> ExcluirPorId(string funcionarioId)
        {
            return _funcionarioService.RemoveOne(funcionarioId);
        }
    }
}
