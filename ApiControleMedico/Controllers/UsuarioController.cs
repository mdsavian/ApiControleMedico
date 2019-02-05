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
    public class UsuarioController : Controller
    {
        private readonly UsuarioService _usuarioService;

        public UsuarioController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        private async Task<List<Usuario>> BuscaAll()
        {
            var xx = await _usuarioService.GetAllAsync();

            return xx.ToList();
        }

        [HttpGet]
        public ActionResult<List<Usuario>> Get()
        {
            var xx = BuscaAll();
            xx.Wait();
            return xx.Result;
        }
    }
}
