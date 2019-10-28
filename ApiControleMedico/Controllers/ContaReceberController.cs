using System.Collections.Generic;
using System.Linq;
using ApiControleMedico.Modelos;
using ApiControleMedico.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiControleMedico.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ContaReceberController : Controller
    {
        private readonly ContaReceberService _contaReceberService;

        public ContaReceberController(ContaReceberService contaReceberService)
        {
            _contaReceberService = contaReceberService;
        }

        [HttpGet]
        public ActionResult<List<ContaReceber>> Get()
        {
            var lista = _contaReceberService.GetAll();
            return lista.ToList();
        }

        [HttpGet, Route("buscarPorId/{contaReceberId}")]
        public ActionResult<ContaReceber> BuscarPorId(string contaReceberId)
        {
            return _contaReceberService.GetOne(contaReceberId);
        }


        [HttpPost]
        public ActionResult<ContaReceber> Salvar(ContaReceber contaReceber)
        {
            return _contaReceberService.SaveOne(contaReceber);
        }

        [HttpDelete, Route("excluirPorId/{contaReceberId}")]
        public ActionResult<bool> ExcluirPorId(string contaReceberId)
        {
            return _contaReceberService.RemoveOne(contaReceberId);
        }
    }
}
