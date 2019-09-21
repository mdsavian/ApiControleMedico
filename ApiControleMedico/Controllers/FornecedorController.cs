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
    public class FornecedorController : Controller
    {   
        private readonly FornecedorService _fornecedorService;

        public FornecedorController(FornecedorService fornecedorService)
        {
            _fornecedorService = fornecedorService;
        }

        [HttpGet]
        public List<Fornecedor> Get()
        {
            var fornecedors = _fornecedorService.GetAll();
            return fornecedors.ToList();
        }

        [HttpPost]
        public ActionResult<Fornecedor> Salvar(Fornecedor fornecedor)
        {
            var fornecedorRetorno = _fornecedorService.SaveOne(fornecedor);
            return fornecedorRetorno;
        }

        [HttpGet, Route("buscarPorId/{fornecedorId}")]
        public ActionResult<Fornecedor> BuscarPorId(string fornecedorId)
        {
            return _fornecedorService.GetOne(fornecedorId);
        }

        [HttpDelete, Route("excluirPorId/{fornecedorId}")]
        public ActionResult<bool> ExcluirPorId(string fornecedorId)
        {
            return _fornecedorService.RemoveOne(fornecedorId);
        }
    }
}
