﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    }
}
