﻿using System.Collections.Generic;
using System.Linq;
using ApiControleMedico.Modelos;
using ApiControleMedico.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiControleMedico.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ConvenioController : Controller
    {
        private readonly ConvenioService _convenioService;
        private readonly ConvenioMedicoService _convenioMedicoService;

        public ConvenioController(ConvenioService convenioService, ConvenioMedicoService convenioMedicoService)
        {
            _convenioService = convenioService;
            _convenioMedicoService = convenioMedicoService;
        }

        [HttpGet]
        public ActionResult<List<Convenio>> Get()
        {
            var lista = _convenioService.GetAllAsync();
            return lista.Result.ToList();
        }

        [HttpPost]
        public ActionResult<Convenio> Salvar(Convenio convenio)
        {
            return _convenioService.SaveOneAsync(convenio).Result;
        }
    }
}
