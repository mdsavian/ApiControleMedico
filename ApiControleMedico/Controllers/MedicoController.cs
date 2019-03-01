using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiControleMedico.Modelos;
using ApiControleMedico.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

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
            medico.Convenios.Add(new ConvenioMedico
            {
                ConvenioId = "5c7034386f341e38b0f6b53e",
                MedicoId = medico.Id,
                Id = ObjectId.GenerateNewId().ToString(),

            });
            var medicoRetorno =  _medicoService.SaveOneAsync(medico);
            return medicoRetorno.Result;
        } 

        [HttpGet, Route("buscarPorId/{medicoId}")]
        public ActionResult<Medico> BuscarPorId(string medicoId)
        {
            return _medicoService.GetOneAsync(medicoId).Result;
        }

        [HttpPost, Route("excluirPorId/{medicoId}")]
        public ActionResult<bool> ExcluirPorId(string medicoId)
        {
            return _medicoService.RemoveOneAsync(medicoId).Result;
        }
    }
}
