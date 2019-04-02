using System.Collections.Generic;
using System.Linq;
using ApiControleMedico.Modelos;
using ApiControleMedico.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiControleMedico.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class MedicoController : Controller
    {
        private readonly MedicoService _medicoService;

        public MedicoController(MedicoService medicoService)
        {
            _medicoService = medicoService;
        }

        [HttpGet]
        public List<Medico> Get()
        {
            var medicos = _medicoService.GetAllAsync();
            return medicos.Result.ToList();
        }

        [HttpPost]
        public ActionResult<Medico> Salvar(Medico medico)
        {
            var medicoRetorno =  _medicoService.SaveOneAsync(medico);
            return medicoRetorno.Result;
        } 

        [HttpGet, Route("buscarPorId/{medicoId}")]
        public ActionResult<Medico> BuscarPorId(string medicoId)
        {
            return _medicoService.GetOneAsync(medicoId).Result;
        }

        [HttpDelete, Route("excluirPorId/{medicoId}")]
        public ActionResult<bool> ExcluirPorId(string medicoId)
        {
            return _medicoService.RemoveOneAsync(medicoId).Result;
        }

        [HttpPost, Route("buscarMedicoUsuario")]
        public ActionResult<Medico> BuscarMedicoUsuario(Usuario usuario)
        {
            return _medicoService.BuscarMedicoUsuario(usuario);
        }
    }
}
