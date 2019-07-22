using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiControleMedico.Modelos;
using ApiControleMedico.Modelos.NaoPersistidos;
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

        [HttpPost, Route("alterarSenha")]
        public ActionResult<Usuario> AlterarSenha(AlteraSenha alteraSenha)
        {
            return _usuarioService.AlterarSenha(alteraSenha);
        }
        private List<Usuario> BuscaAll()
        {
            var todosUsuarios = _usuarioService.GetAll();

            return todosUsuarios.ToList();
        }

        [HttpGet]
        public ActionResult<List<Usuario>> Get()
        {
            var todosUsuarios = BuscaAll();            
            return todosUsuarios;
        }
    }
}
