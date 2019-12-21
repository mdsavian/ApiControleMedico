using System.Collections.Generic;
using System.Linq;
using ApiControleMedico.Modelos;
using ApiControleMedico.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiControleMedico.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ProntuarioController : Controller
    {   
        private readonly ProntuarioService _prontuarioService;
        

        public ProntuarioController(ProntuarioService prontuarioService)
        {
            _prontuarioService = prontuarioService;
        }

        [HttpGet]
        public List<Prontuario> Get()
        {
            var prontuarios = _prontuarioService.GetAll();
            return prontuarios.ToList();
        }

        [HttpPost]
        public ActionResult<Prontuario> Salvar(Prontuario prontuario)
        {
            var prontuarioRetorno = _prontuarioService.SaveOne(prontuario);
            return prontuarioRetorno;
        }

        [HttpGet, Route("downloadArquivo/{idArquivo}")]
        public ActionResult<JsonResult> DownloadArquivo(string idArquivo)
        {
            byte[] retorno = null;

            retorno = _prontuarioService.DownloadArquivo(idArquivo);

            if (retorno != null)
            {
                return Json(System.Convert. ToBase64String(retorno));
            }

            return null;
        }

        [HttpPost, DisableRequestSizeLimit, Route("salvarArquivos")]
        public ActionResult<Prontuario> SalvarArquivos()
        {
            try
            {
                var file = Request.Form.Files[0];
                return _prontuarioService.SalvarArquivos(file);
            }
            catch (System.Exception ex)
            {
                return Json("Falha ao fazer upload: " + ex.Message);
            }
        }

        [HttpGet, Route("buscarPorPaciente/{pacienteId}")]
        public ActionResult<Prontuario> BuscarPorPaciente(string pacienteId)
        {
            return _prontuarioService.BuscarPorPaciente(pacienteId);
        }

        [HttpDelete, Route("excluirPorId/{prontuarioId}")]
        public ActionResult<bool> ExcluirPorId(string prontuarioId)
        {
            return _prontuarioService.RemoveOne(prontuarioId);
        }
    }
}
