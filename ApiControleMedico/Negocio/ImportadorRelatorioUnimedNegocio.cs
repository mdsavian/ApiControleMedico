using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf.parser;

namespace ApiControleMedico.Negocio
{

    //1 inch = 72 points and 1 cm = 1/2.54 inch = 0.3937 inch = 28.3 points.
    //where,
    //(lower left x coordinate)  llx = margin from left.
    //(lower left y coordinate)  lly = margin from bottom(bottom of rectangle)
    //(upper right x coordinate) urx = width of article
    //(upper right y coordinate) ury = margin from bottom of upper boundary of article.

    public static class ImportadorRelatorioUnimedNegocio
    {
        public static void ProcessarArquivoPdf(string strPath)
        {
            using (PdfReader reader = new PdfReader(strPath))
            {
                StringBuilder text = new StringBuilder();

                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    var datas = Data(reader, i);
                    var datasa = Data(reader, i);
                    var nomePaciente = NomePaciente(reader, i);
                    var tipoPlano = TipoPlano(reader, i);
                    var servico = Servico(reader, i);
                    var movimento = Movimento(reader, i);
                    

                    ProcessarDadosPagina(nomePaciente, tipoPlano, servico, movimento);
                }
            }
        }


        private static void ProcessarDadosPagina(string[] nomePaciente, string[] tipoPlano, string[] servico, string[] movimento)
        {
            for (var i = 0; i <= nomePaciente.Length; i++)
            {

            }
        }

        private static string[] Data(PdfReader reader, int pagina)
        {
            pagina = 1;
            RenderFilter[] filters = new RenderFilter[1];
            LocationTextExtractionStrategy regionFilter = new LocationTextExtractionStrategy();
            filters[0] = new RegionTextRenderFilter(new Rectangle(85, 25, 600, 210));
            FilteredTextRenderListener strategy = new FilteredTextRenderListener(regionFilter, filters);

            string result = PdfTextExtractor.GetTextFromPage(reader, pagina, strategy);
            return result.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        }

        private static string[] NomePaciente(PdfReader reader, int pagina)
        {
            RenderFilter[] filters = new RenderFilter[1];
            LocationTextExtractionStrategy regionFilter = new LocationTextExtractionStrategy();
            filters[0] = new RegionTextRenderFilter(new Rectangle(85, 300, 600, 315));
            FilteredTextRenderListener strategy = new FilteredTextRenderListener(regionFilter, filters);
            string result = PdfTextExtractor.GetTextFromPage(reader, pagina, strategy);
            return result.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        }

        private static string[] TipoPlano(PdfReader reader, int pagina)
        {
            RenderFilter[] filters = new RenderFilter[1];
            LocationTextExtractionStrategy regionFilter = new LocationTextExtractionStrategy();
            filters[0] = new RegionTextRenderFilter(new Rectangle(85, 400, 600, 425));
            FilteredTextRenderListener strategy = new FilteredTextRenderListener(regionFilter, filters);
            string result = PdfTextExtractor.GetTextFromPage(reader, pagina, strategy);
            return result.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        }

        private static string[] Movimento(PdfReader reader, int pagina)
        {
            RenderFilter[] filters = new RenderFilter[1];
            LocationTextExtractionStrategy regionFilter = new LocationTextExtractionStrategy();
            filters[0] = new RegionTextRenderFilter(new Rectangle(85, 500, 600, 535));
            FilteredTextRenderListener strategy = new FilteredTextRenderListener(regionFilter, filters);
            string result = PdfTextExtractor.GetTextFromPage(reader, pagina, strategy);

            return result.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        }

        private static string[] Servico(PdfReader reader, int pagina)
        {
            RenderFilter[] filters = new RenderFilter[1];
            LocationTextExtractionStrategy regionFilter = new LocationTextExtractionStrategy();
            filters[0] = new RegionTextRenderFilter(new Rectangle(85, 600, 600, 645));
            FilteredTextRenderListener strategy = new FilteredTextRenderListener(regionFilter, filters);
            string result = PdfTextExtractor.GetTextFromPage(reader, pagina, strategy);
            return result.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        }
        
        
    }
}
