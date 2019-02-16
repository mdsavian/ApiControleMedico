using System.IO;
using System.Net.Http.Headers;
using ApiControleMedico.Modelos.NaoPersistidos;
using ApiControleMedico.Negocio;
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
                {
                    Directory.CreateDirectory(newPath);
                }

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


       

        

        
    }
}