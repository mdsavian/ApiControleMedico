using System.Collections.Generic;
using ApiControleMedico.Modelos;
using ApiControleMedico.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiControleMedico.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AppController : Controller
    {   
        private readonly AppService _appService;

        public AppController(AppService appService)
        {
            _appService = appService;
        }


        [HttpGet]
        public void Get()
        {
            
        }

        [HttpGet, Route("buscarClinicasUsuario/{usuarioId}")]
        public ActionResult<List<Clinica>> BuscarClinicasUsuario(string usuarioid)
        {
            return _appService.BuscarClinicasUsuario(usuarioid);
        }

       }
}
