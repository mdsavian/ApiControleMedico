using ApiControleMedico.Modelos.NaoPersistidos;
using ApiControleMedico.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiControleMedico.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class EnderecoController : Controller
    {
        private readonly EnderecoService _enderecoService;

        public EnderecoController(EnderecoService enderecoService)
        {
            _enderecoService = enderecoService;
        }
        
        [HttpGet, Route("buscaCep/{cep}")]
        public ActionResult<Endereco> Get(string cep)
        {
            return this._enderecoService.BuscaEnderecoWebService(cep);
        }
    }
}
