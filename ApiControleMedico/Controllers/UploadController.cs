using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
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
        public ActionResult UploadFile()
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

                    ProcessarArquivoPdf(fullPath);
                }
                return Json("Upload Successful.");
            }
            catch (System.Exception ex)
            {
                return Json("Upload Failed: " + ex.Message);
            }
        }


        public void ProcessarArquivoPdf(string strPath)
        {
            using (PdfReader reader = new PdfReader(strPath))
            {
                StringBuilder text = new StringBuilder();

                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    var nomePaciente = NomePaciente(reader);
                    var tipoPlano = TipoPlano(reader);
                    var servico = Servico(reader);
                    var movimento = Movimento(reader);
//                    i = 70;
                }
            }
        }
        private string Movimento(PdfReader reader, int pagina)
        {
            RenderFilter[] filters = new RenderFilter[1];
            LocationTextExtractionStrategy regionFilter = new LocationTextExtractionStrategy();
            filters[0] = new RegionTextRenderFilter(new Rectangle(85, 500, 600, 535));
            FilteredTextRenderListener strategy = new FilteredTextRenderListener(regionFilter, filters);
            string result = PdfTextExtractor.GetTextFromPage(reader, pagina, strategy);



            return result;
        }
        private string Servico(PdfReader reader, int pagina)
        {
            RenderFilter[] filters = new RenderFilter[1];
            LocationTextExtractionStrategy regionFilter = new LocationTextExtractionStrategy();
            filters[0] = new RegionTextRenderFilter(new Rectangle(85, 600, 600, 645));
            FilteredTextRenderListener strategy = new FilteredTextRenderListener(regionFilter, filters);
            string result = PdfTextExtractor.GetTextFromPage(reader, pagina, strategy);
            return result;
        }
        private string TipoPlano(PdfReader reader, int pagina)
        {
            RenderFilter[] filters = new RenderFilter[1];
            LocationTextExtractionStrategy regionFilter = new LocationTextExtractionStrategy();
            filters[0] = new RegionTextRenderFilter(new Rectangle(85, 400, 600, 425));
            FilteredTextRenderListener strategy = new FilteredTextRenderListener(regionFilter, filters);
            string result = PdfTextExtractor.GetTextFromPage(reader, pagina, strategy);
            return result;
        }
        private string NomePaciente(PdfReader reader, int pagina)
        {
            RenderFilter[] filters = new RenderFilter[1];
            LocationTextExtractionStrategy regionFilter = new LocationTextExtractionStrategy();
            filters[0] = new RegionTextRenderFilter(new Rectangle(85, 300, 600, 315));
            FilteredTextRenderListener strategy = new FilteredTextRenderListener(regionFilter, filters);
            string result = PdfTextExtractor.GetTextFromPage(reader, pagina , strategy);
            return result;
        }
    


    }
}