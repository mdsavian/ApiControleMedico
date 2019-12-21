using System.IO;
using System.Net.Http.Headers;
using ApiControleMedico.Modelos.NaoPersistidos;
using ApiControleMedico.Negocio;
using ApiControleMedico.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Path = System.IO.Path;

namespace ApiControleMedico.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;

        public UploadController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }


        [HttpPost, DisableRequestSizeLimit]
        public ActionResult<DadosRelatorioUnimed> UploadFile()
        {
            try
            {
                var file = Request.Form.Files[0];
                string folderName = "Upload";
                string webRootPath = _hostingEnvironment.ContentRootPath;
                string newPath = Path.Combine(webRootPath, folderName);

                if (!Directory.Exists(newPath))
                    Directory.CreateDirectory(newPath);


                if (file.Length > 0)
                {
                    string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    string fullPath = Path.Combine(newPath, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    return Json(ImportadorRelatorioUnimedNegocio.ProcessarArquivoPdf(fullPath));
                }

                return Json("Falha ao fazer upload");
            }
            catch (System.Exception ex)
            {
                return Json("Falha ao fazer upload: " + ex.Message);
            }
        }

        [HttpPost, DisableRequestSizeLimit, Route("salvarImagem")]
        public void SalvarImagem()
        {
            try
            {
                var file = Request.Form.Files["image"];

                string folderName = "Upload";
                string webRootPath = _hostingEnvironment.ContentRootPath;
                string newPath = Path.Combine(webRootPath, folderName);

                if (!Directory.Exists(newPath))
                    Directory.CreateDirectory(newPath);

                if (file.Length > 0)
                {
                    string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

                    var nome = fileName.Split('-');
                    var entidade = nome[0];
                    var id = nome[1].Split('.')[0];

                    string fullPath = Path.Combine(newPath, $"{fileName}");

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                        file.CopyTo(stream);

                    switch (entidade)
                    {
                        case "paciente":
                            {
                                new PacienteService().SalvarFoto(id, fileName, fullPath);
                                break;
                            }
                        case "medico":
                            {
                                new MedicoService().SalvarFoto(id, fileName, fullPath);
                                break;
                            }
                        case "clinica":
                            {
                                new ClinicaService().SalvarLogo(id, fileName, fullPath);
                                break;
                            }
                        case "funcionario":
                            {
                                new FuncionarioService().SalvarFoto(id, fileName, fullPath);
                                break;
                            }
                    }

                    System.IO.File.Delete(fullPath);
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        [HttpGet, Route("downloadImagem")]
        public ActionResult<JsonResult> DownloadImagem([FromQuery]string id, [FromQuery] string entidade)
        {
            byte[] retorno = null;
            switch (entidade)
            {
                case "paciente":
                    {
                        retorno = new PacienteService().DownloadFoto(id);
                        break;
                    }
                case "medico":
                    {
                        retorno = new MedicoService().DownloadFoto(id);
                        break;
                    }
                case "clinica":
                    {
                        retorno = new ClinicaService().DownloadLogo(id);
                        break;
                    }
                case "funcionario":
                    {
                        retorno = new FuncionarioService().DownloadFoto(id);
                        break;
                    }
            }

            if (retorno != null)
            {
                return Json(System.Convert.ToBase64String(retorno));
            }

            return null;
        }

    }
}