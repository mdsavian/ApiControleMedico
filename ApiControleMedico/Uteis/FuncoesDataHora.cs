using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiControleMedico.Uteis
{
    public static class FuncoesDataHora
    {

        public static DateTime? UnirDataHora(this DateTime? data, TimeSpan? hora)
        {
            if (data == null || hora == null)
                return data;

            return new DateTime(data.Value.Year, data.Value.Month, data.Value.Day, hora.Value.Hours, hora.Value.Minutes, hora.Value.Seconds);
        }

        public static DateTime UnirDataHora(this DateTime data, TimeSpan hora)
        {
            return new DateTime(data.Year, data.Month, data.Day, hora.Hours, hora.Minutes, hora.Seconds);
        }

        public static DateTime UltimaHoraDia(this DateTime data)
        {
            return new DateTime(data.Year, data.Month, data.Day, 23, 59, 59);
        }

        public static DateTime PrimeiroDiaDiaMes(this DateTime data)
        {
            return new DateTime(data.Year, data.Month, 01);
        }

        public static DateTime UltimoDiaMes(this DateTime data)
        {
            return new DateTime(data.Year, data.Month, DateTime.DaysInMonth(data.Year, data.Month));
        }

        public static DateTime PrimeiroDiaMes(this DateTime data)
        {
            return new DateTime(data.Year, data.Month, 01);
        }

        public static DateTime InicioDaSemana(this DateTime data, DayOfWeek diaInicio = DayOfWeek.Sunday)
        {
            int diff = data.DayOfWeek - diaInicio;
            if (diff < 0)
            {
                diff += 7;
            }
            return data.AddDays(-1 * diff).Date;
        }

        public static string FormatarHora24(this DateTime? fechaHora)
        {
            if (!fechaHora.HasValue)
                return "";

            return $"{(DateTime)fechaHora:HH:mm}";
        }

        public static string FormatarHora24(this DateTime fechaHora)
        {
            return $"{fechaHora:HH:mm}";
        }

        public static string FormatarHora(this DateTime fechaHora)
        {
            return $"{fechaHora:HH:mm:ss}";
        }

        public static string FormatarDiaMesAno(this DateTime? data)
        {
            if (!data.HasValue)
                return "";

            return data.Value.ToString("dd/MM/yyyy");
        }

        public static string FormatarDiaMesAnoHora(this DateTime? data)
        {
            if (!data.HasValue)
                return "";

            return data.Value.ToString("dd/MM/yyyy HH:mm:ss");
        }

        public static string FormatarDiaMesAnoHoraSemSegundos(this DateTime? data)
        {
            if (!data.HasValue)
                return "";

            return data.Value.ToString("dd/MM/yyyy HH:mm");
        }

        public static string FormatarDiaMesAnoHora(this DateTime data)
        {
            return data.ToString("dd/MM/yyyy HH:mm:ss");
        }

        public static string FormatarDiaMesAnoHoraSemSegundos(this DateTime data)
        {
            return data.ToString("dd/MM/yy HH:mm");
        }

        public static string FormatarDiaMesAnoAbrevHora(this DateTime data)
        {
            return data.ToString("dd/MM/yy HH:mm:ss");
        }


        public static string FormatarDiaMesAnoHoraMilesimos(this DateTime data)
        {
            return data.ToString("dd/MM/yyyy HH:mm:ss fff");
        }
        public static string FormatarDiaMesAnoHoraPadraAbrasf(this DateTime? data)
        {
            return data?.ToString("dd/MM/yyyyTHH:mm:ss") ?? "";
        }

       

        /// <summary>
        ///  Formata a data utilizando o padrão dd/MM/yyyy
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string FormatarDiaMesAno(this DateTime data)
        {
            return data.ToString("dd/MM/yyyy");
        }

        public static string FormatarHora24(this TimeSpan? timeSpan)
        {
            return timeSpan.HasValue ? $"{timeSpan.Value.Hours:D2}:{timeSpan.Value.Minutes:D2}" : "";
        }

        public static string FormatarHora24(this TimeSpan timeSpan)
        {
            return $"{timeSpan.Hours:D2}:{timeSpan.Minutes:D2}";
        }

        public static string FormatarDiaHoraMinutosSegundos(this TimeSpan timeSpan)
        {
            var dias = timeSpan.Days <= 0 ? string.Empty : timeSpan.Days == 1 ? "1 dia" : $"{timeSpan.Days} dias";
            return $"{dias} {timeSpan.Hours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
        }

       

        public static string FormatarHoraMinutosSegundos(this TimeSpan timeSpan)
        {
            return $"{timeSpan.Hours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
        }

        public static string FormatarHoraMinutos(this TimeSpan timeSpan)
        {
            return $"{timeSpan.Hours:D2}:{timeSpan.Minutes:D2}";
        }

        public static string FormatarMinutosESegundos(this TimeSpan timeSpan)
        {
            return $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
        }

        public static DateTime BuscarDataPassadaPorDiaSemana(this DateTime dataReferencia, DayOfWeek diaSemanaa)
        {
            var dataResultado = dataReferencia.AddDays(-1);

            while (true)
            {
                if (dataResultado.DayOfWeek == diaSemanaa)
                    break;

                dataResultado = dataReferencia.AddDays(-1);
            }

            return dataResultado;
        }

        public static DateTime DiaDaSemanaReferenteAoMesAnterior(this DateTime dataReferencia)
        {
            var datasCalculo = new List<DateTime>();
            var dataUltimoMes = dataReferencia.AddMonths(-1).PrimeiroDiaMes();

            while (dataUltimoMes <= dataReferencia)
            {
                datasCalculo.Add(dataUltimoMes);
                dataUltimoMes = dataUltimoMes.AddDays(1);
            }

            var diaSemana = datasCalculo.Count(c => c.Date >= dataReferencia.PrimeiroDiaMes() && c.DayOfWeek == dataReferencia.DayOfWeek);
            var diasDaSemana = datasCalculo.Where(c => c.DayOfWeek == dataReferencia.DayOfWeek).ToList();

            return diasDaSemana[diaSemana - 1];
        }

        public static string FormatarMesAnoPorExtenso(this DateTime data)
        {
            var result = string.Empty;
            switch (data.Month)
            {
                case 1:
                    result = "JANEIRO";
                    break;
                case 2:
                    result = "FEVEREIRO";
                    break;
                case 3:
                    result = "MARÇO";
                    break;
                case 4:
                    result = "ABRIL";
                    break;
                case 5:
                    result = "MAIO";
                    break;
                case 6:
                    result = "JUNHO";
                    break;
                case 7:
                    result = "JULHO";
                    break;
                case 8:
                    result = "AGOSTO";
                    break;
                case 9:
                    result = "SETEMBRO";
                    break;
                case 10:
                    result = "OUTUBRO";
                    break;
                case 11:
                    result = "NOVEMBRO";
                    break;
                case 12:
                    result = "DEZEMBRO";
                    break;
            }

            result += $"/{data:yy}";
            return result;
        }


    }
}
