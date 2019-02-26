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
    public class LoginController : Controller
    {
        private readonly LoginService _loginService;

        public LoginController(LoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpGet]
        public bool Get()
        {
            return true;
        }

        [HttpPost]
        public ActionResult<Usuario> Login(Usuario usuario)
        {
            return _loginService.ValidarLogin(usuario);
        }

        [HttpGet, Route("validaUsuario/{token}")]
        public ActionResult<bool> ValidaUsuario(string token)
        {
            return false;

        }
    }
}
