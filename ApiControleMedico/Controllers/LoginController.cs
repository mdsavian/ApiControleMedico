using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiControleMedico.Modelos;
using ApiControleMedico.Services;
using ApiControleMedico.Uteis;
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

        [HttpPost, Route("validaUsuario")]
        public ActionResult<bool> ValidaUsuario(Usuario usuario)
        {
            if (!string.IsNullOrEmpty(usuario.UltimoLogin))
            {
                var datahora = usuario.UltimoLogin.ToDateTime();

                if (DateTime.Now.FormatarDiaMesAnoHora().ToDateTime().Subtract(datahora).TotalMinutes > 180)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
