using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiControleMedico.Modelos.NaoPersistidos;
using ApiControleMedico.Uteis;
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

        private static int _llx;
        private static readonly int _urx = 600;

        public static List<DadosRelatorioUnimed> ProcessarArquivoPdf(string strPath)
        {
            List<DadosRelatorioUnimed> dados = new List<DadosRelatorioUnimed>();

            using (PdfReader reader = new PdfReader(strPath))
            {

                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    _llx = i != 1 ? 25 : 85;
                    var primeirasColunas = PrimeirasColunas(reader, i);
                    var nomePaciente = NomePaciente(reader, i);
                    var tipoPlano = TipoPlano(reader, i);
                    var movimento = Movimento(reader, i);
                    var servico = Servico(reader, i);
                    var ultimasColunas = UltimasColunas(reader, i);

                    dados.AddRange(ProcessarDadosPagina(primeirasColunas, nomePaciente, tipoPlano, movimento, servico, ultimasColunas));
                }
            }

            return dados;
        }


        private static List<DadosRelatorioUnimed> ProcessarDadosPagina(string[] primeirasColunas, string[] nomePaciente, string[] tipoPlano, string[] movimento, string[] servico, string[] ultimasColunas)
        {
            List<DadosRelatorioUnimed> dados = new List<DadosRelatorioUnimed>();
            for (var i = 0; i < nomePaciente.Length; i++)
            {
                var primeirasColunasSplit = primeirasColunas[i].Split(' ');
                var ultimasColunasSplit = ultimasColunas[i].Split(' ');
                try
                {
                    dados.Add(new DadosRelatorioUnimed
                    {
                        Data = $"{primeirasColunasSplit[0]} {primeirasColunasSplit[1]}".ToDateTime(),
                        Documento = primeirasColunasSplit[4].ToLong(),
                        Carteira = primeirasColunasSplit[5].ToLong(),
                        Beneficiario = nomePaciente[i],
                        TipoPlano = tipoPlano[i],
                        CodigoMovimento = movimento[i].ToLong(),
                        Servico = i < nomePaciente.Length - 1? TratarTextoServico(servico[i], servico[i + 1]) : servico[i],
                        Quantidade = ultimasColunasSplit[0].ToDecimal(),
                        ValorProduto = ultimasColunasSplit[1].ToDecimal(),
                        ValorParticipacao = ultimasColunasSplit[2].ToDecimal(),
                        PrevPagamento = ultimasColunasSplit[3].ToDecimal(),


                    });
                }
                catch (Exception ex)
                {
                    throw new Exception($"Houve um erro ao processar os dados {ex.Message}");
                }

            }
            return dados;
        }

        private static string TratarTextoServico(string s, string s1)
        {
            if (s.Contains('(') && !s.Contains(')') && s1.Contains(')') && !s1.Contains('('))
                return s + s1;

            return s;
        }


        private static string[] PrimeirasColunas(PdfReader reader, int pagina)
        {

            RenderFilter[] filters = new RenderFilter[1];
            LocationTextExtractionStrategy regionFilter = new LocationTextExtractionStrategy();
            filters[0] = new RegionTextRenderFilter(new Rectangle(_llx, 30, _urx, 150));
            FilteredTextRenderListener strategy = new FilteredTextRenderListener(regionFilter, filters);
            string result = PdfTextExtractor.GetTextFromPage(reader, pagina, strategy);
            return result.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        }

        private static string[] NomePaciente(PdfReader reader, int pagina)
        {
            PrimeirasColunas(reader, pagina);
            RenderFilter[] filters = new RenderFilter[1];
            LocationTextExtractionStrategy regionFilter = new LocationTextExtractionStrategy();
            filters[0] = new RegionTextRenderFilter(new Rectangle(_llx, 300, _urx, 315));
            FilteredTextRenderListener strategy = new FilteredTextRenderListener(regionFilter, filters);
            string result = PdfTextExtractor.GetTextFromPage(reader, pagina, strategy);
            return result.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        }

        private static string[] TipoPlano(PdfReader reader, int pagina)
        {
            RenderFilter[] filters = new RenderFilter[1];

            LocationTextExtractionStrategy regionFilter = new LocationTextExtractionStrategy();
            filters[0] = new RegionTextRenderFilter(new Rectangle(_llx, 400, _urx, 425));
            FilteredTextRenderListener strategy = new FilteredTextRenderListener(regionFilter, filters);
            string result = PdfTextExtractor.GetTextFromPage(reader, pagina, strategy);
            return result.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        }

        private static string[] Movimento(PdfReader reader, int pagina)
        {
            RenderFilter[] filters = new RenderFilter[1];
            LocationTextExtractionStrategy regionFilter = new LocationTextExtractionStrategy();
            filters[0] = new RegionTextRenderFilter(new Rectangle(_llx, 500, _urx, 535));
            FilteredTextRenderListener strategy = new FilteredTextRenderListener(regionFilter, filters);
            string result = PdfTextExtractor.GetTextFromPage(reader, pagina, strategy);
            return result.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        }

        private static string[] Servico(PdfReader reader, int pagina)
        {
            RenderFilter[] filters = new RenderFilter[1];
            LocationTextExtractionStrategy regionFilter = new LocationTextExtractionStrategy();
            filters[0] = new RegionTextRenderFilter(new Rectangle(_llx, 600, _urx, 645));
            FilteredTextRenderListener strategy = new FilteredTextRenderListener(regionFilter, filters);
            string result = PdfTextExtractor.GetTextFromPage(reader, pagina, strategy);
            return result.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        }

        private static string[] UltimasColunas(PdfReader reader, int pagina)
        {
            var filters = new RenderFilter[1];
            filters[0] = new RegionTextRenderFilter(new Rectangle(_llx, 705, _urx, 805));
            var regionFilter = new LocationTextExtractionStrategy();
            var strategy = new FilteredTextRenderListener(regionFilter, filters);
            var result = PdfTextExtractor.GetTextFromPage(reader, pagina, strategy);
            return result.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        }


    }
}
