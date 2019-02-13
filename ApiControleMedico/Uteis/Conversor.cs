using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ApiControleMedico.Modelos.Enums;

namespace ApiControleMedico.Uteis
{
    public static class Conversor
    {
        /// <summary>
        /// Metodo: Converte valor para decimal.
        /// </summary>
        /// <param name="value">object: valor a ser convertido, podendo ser de qualquer tipo</param>
        /// <returns>decimal: retorna valor convertido, se valor não puder ser convertido retorna 0</returns>
        public static decimal ToDecimal(this object value)
        {
            decimal result = 0;

            if (value != null && value != DBNull.Value)
            {
                if (value is decimal || value is int || value is long || value is byte)
                    result = Convert.ToDecimal(value);
                else
                {
                    if (decimal.TryParse(value.ToString(), NumberStyles.Any, CultureInfo.GetCultureInfo("pt-BR"),
                        out var valorConvertido))
                        result = valorConvertido;
                }
            }

            return result;
        }

        /// <summary>
        ///  Conta a quantidade de linhas contidas em uma string.
        /// </summary>
        /// <param name="s"> Texto. </param>
        /// <returns></returns>
        public static long LinesCount(string s)
        {
            long count = 0;
            int position = 0;
            while ((position = s.IndexOf('\n', position)) != -1)
            {
                count++;
                position++; // Skip this occurrence!
            }

            return count;
        }

        /// <summary>
        /// Metodo: Converte valor para decimal.
        /// </summary>
        /// <param name="value">object: valor a ser convertido, podendo ser de qualquer tipo. </param>
        /// <param name="format"> Formato de retorno do decimar ex: (###,###,###,##0.00). </param>
        /// <returns>decimal: retorna valor convertido, se valor não puder ser convertido retorna 0. </returns>
        public static decimal ToDecimalFormat(this object value, string format = "###,###,###,##0.00")
        {
            decimal result = 0;

            if (value != null && value != DBNull.Value)
            {
                if (value is decimal || value is int || value is long || value is byte)
                    result = Convert.ToDecimal(value);
                else
                {
                    decimal valorConvertido;
                    if (decimal.TryParse(value.ToString(), out valorConvertido))
                        result = valorConvertido;
                }
            }

            if (result != 0)
            {
                result = result.ToString(format).ToDecimal();
            }

            return result;
        }

        /// <summary>
        /// Metodo: Converte valor para decimal ou nulo.
        /// </summary>
        /// <param name="value">object: valor a ser convertido, podendo ser de qualquer tipo</param>
        /// <returns>decimal: retorna valor convertido, se valor não puder ser convertido retorna null</returns>
        public static decimal? ToDecimalOrNull(this object value)
        {
            decimal? result = null;

            if (value != null && value != DBNull.Value)
            {
                if (value is decimal || value is int || value is long || value is byte)
                    result = Convert.ToDecimal(value);
                else
                {
                    decimal valorConvertido;
                    if (decimal.TryParse(value.ToString(), out valorConvertido))
                        result = valorConvertido;
                }
            }

            return result;
        }

        /// <summary>
        /// Metodo: Converte valor para byte.
        /// </summary>
        /// <param name="value">object: valor a ser convertido, podendo ser de qualquer tipo</param>
        /// <returns>byte: retorna valor convertido, se valor não puder ser convertido retorna 0</returns>
        public static byte ToByte(object value)
        {
            byte result = 0;

            if (value != null && value != DBNull.Value)
            {
                if (value is byte)
                    result = Convert.ToByte(value);
                else
                {
                    byte valorConvertido;
                    if (byte.TryParse(value.ToString(), out valorConvertido))
                        result = valorConvertido;
                }
            }

            return result;
        }


        /// <summary>
        /// Metodo: Converte valor para byte ou nulo.
        /// </summary>
        /// <param name="value">object: valor a ser convertido, podendo ser de qualquer tipo</param>
        /// <returns>byte: retorna valor convertido, se valor não puder ser convertido retorna null</returns>
        public static byte? ToByteOrNull(object value)
        {
            byte? result = (byte?) null;

            if (value != null && value != DBNull.Value)
            {
                if (value is byte)
                    result = Convert.ToByte(value);
                else
                {
                    byte valorConvertido;
                    if (byte.TryParse(value.ToString(), out valorConvertido))
                        result = valorConvertido;
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static decimal BytesToMegabytes(long bytes)
        {
            return ((bytes / 1024M) / 1024M);
        }

        /// <summary>
        /// Metodo: Converte objeto para long.
        /// </summary>
        /// <param name="value">object: valor a ser convertido, podendo ser de qualquer tipo </param>
        /// <returns>long: retorna valor convertido, se valor não puder ser convertido retorna 0</returns>
        public static long ToLong(this object value)
        {
            long result = 0;

            if (value == null || value == DBNull.Value) return result;

            if (value is long)
                result = Convert.ToInt64(value);
            else
            {
                long valorConvertido;
                if (long.TryParse(value.ToString(), out valorConvertido))
                    result = valorConvertido;
            }

            return result;
        }

        /// <summary>
        /// Metodo: Converte objeto para bool.
        /// </summary>
        /// <param name="value">object: valor a ser convertido, podendo ser de qualquer tipo </param>
        /// <returns>bool: retorna valor convertido, se valor não puder ser convertido retorna false</returns>
        public static bool ToBool(this object value)
        {
            bool result = false;

            if (value == null || value == DBNull.Value) return result;

            if (value is bool)
                result = Convert.ToBoolean(value);
            else
            {
                bool valorConvertido;
                if (bool.TryParse(value.ToString(), out valorConvertido))
                    result = valorConvertido;
            }

            if (!result && value.ToString() == "1")
                result = true;

            return result;
        }

        /// <summary>
        /// Metodo: Converte objeto para long ou null.
        /// </summary>
        /// <param name="value">object: valor a ser convertido, podendo ser de qualquer tipo </param>
        /// <returns>long: retorna valor convertido, se valor não puder ser convertido retorna null</returns>
        public static long? ToLongOrNull(this object value)
        {
            long? result = null;

            if (value != null && value != DBNull.Value)
            {
                if (value is long)
                    result = Convert.ToInt64(value);
                else
                {
                    long valorConvertido;
                    if (long.TryParse(value.ToString(), out valorConvertido))
                        result = valorConvertido;
                }
            }

            return result;
        }

        /// <summary>
        /// Metodo: Converte objeto para int.
        /// </summary>
        /// <param name="value">object: valor a ser convertido, podendo ser de qualquer tipo </param>
        /// <returns>Int: retorna valor convertido, se valor não puder ser convertido retorna 0</returns>
        public static int ToInt(this object value)
        {
            int result = 0;

            if (value != null && value != DBNull.Value)
            {
                if (value is int)
                    result = Convert.ToInt32(value);
                else
                {
                    int valorConvertido;
                    if (int.TryParse(value.ToString(), out valorConvertido))
                        result = valorConvertido;
                }
            }

            return result;
        }

        /// <summary>
        /// Metodo: Converte objeto para int ou null.
        /// </summary>
        /// <param name="value">object: valor a ser convertido, podendo ser de qualquer tipo </param>
        /// <returns>int: retorna valor convertido, se valor não puder ser convertido retorna null</returns>
        public static int? ToIntOrNull(this object value)
        {
            int? result = null;

            if (value != null && value != DBNull.Value)
            {
                if (value is int)
                    result = Convert.ToInt32(value);
                else
                {
                    int valorConvertido;
                    if (int.TryParse(value.ToString(), out valorConvertido))
                        result = valorConvertido;
                }
            }

            return result;
        }

        /// <summary>
        /// Metodo: Converte objeto para DateTime.
        /// </summary>
        /// <param name="value">object: valor a ser convertido, podendo ser de qualquer tipo </param>
        /// <returns>DateTime: retorna valor convertido, se valor não puder ser convertido retorna 01/01/1900</returns>
        public static DateTime ToDateTime(this object value)
        {
            var result = DateTime.MinValue;

            if (value == null || value == DBNull.Value)
                return result;

            if (value is DateTime)
                result = Convert.ToDateTime(value);
            else
            {
                if (DateTime.TryParse(value.ToString(), out var valorConvertido))
                    result = valorConvertido;
            }

            return result;
        }

        /// <summary>
        ///  Formata um string para o padrão dd/MM/yyyy
        /// </summary>
        /// <param name="value"> <see cref="string"/> para ser formatada </param>
        /// <returns> <see cref="string"/> formatada. </returns>
        public static string ToDateFormat(this string value)
        {
            var data = value.RemoverCaracteresEspeciais();

            if (data.Length == 1)
            {
                data = data.FormatarString(2, EDirecao.Esquerda, '0');
            }

            if (data.Length == 2)
            {
                data =
                    $"{data}/{DateTime.Today.Month.ToString().FormatarString(2, EDirecao.Esquerda, '0')}/{DateTime.Today.Year}";
            }

            if (data.Length == 3)
            {
                data = $"{data.Substring(0, 2)}/0{data.Substring(2, 1)}/{DateTime.Today.Year}";
            }

            if (data.Length == 4)
            {
                data = $"{data.Substring(0, 2)}/{data.Substring(2, 2)}/{DateTime.Today.Year}";
            }

            else if (data.Length == 8)
            {
                data = $"{data.Substring(0, 2)}/{data.Substring(2, 2)}/{data.Substring(4, 4)}";
            }

            if ((data.ToDateTimeOrNull() == null) || (data.Length == 6 || data.Length == 7) ||
                (data.ToDateTime() < new DateTime(1900, 01, 01)))
            {
                return string.Empty;
            }
            else
            {
                return data;
            }
        }

        /// <summary>
        /// Metodo: Converte objeto para DateTime.
        /// </summary>
        /// <param name="value">object: valor a ser convertido, podendo ser de qualquer tipo </param>
        /// <returns>DateTime: retorna valor convertido, se valor não puder ser convertido retorna 01/01/1900</returns>
        public static DateTime ToDateTime(this DateTime? value)
        {
            DateTime result = DateTime.MinValue;

            if (value.HasValue)
            {
                result = Convert.ToDateTime(value);
            }

            return result;
        }

        /// <summary>
        /// Metodo: Converte objeto para DateTime.
        /// </summary>
        /// <param name="value">object: valor a ser convertido, podendo ser de qualquer tipo </param>
        /// <param name="valorDefault">DateTime: caso não consiga converter usar o valor default </param>>
        /// <returns>DateTime: retorna valor convertido, se valor não puder ser convertido retorna 01/01/1900</returns>        
        public static DateTime ToDateTime(object value, DateTime valorDefault)
        {
            DateTime result = valorDefault;

            if (value != null && value != DBNull.Value)
            {
                if (value is DateTime)
                    result = Convert.ToDateTime(value);
                else
                {
                    DateTime valorConvertido;
                    if (DateTime.TryParse(value.ToString(), out valorConvertido))
                        result = valorConvertido;
                }
            }

            return result;
        }

        /// <summary>
        /// Metodo: Converte objeto para DateTime.
        /// </summary>
        /// <param name="value">object: valor a ser convertido, podendo ser de qualquer tipo </param>
        /// <returns>DateTime: retorna valor convertido, se valor não puder ser convertido retorna 01/01/1900</returns>
        public static DateTime? ToDateTimeOrNull(this object value)
        {
            DateTime? result = null;

            if (value != null && value != DBNull.Value)
            {
                if (value is DateTime)
                    result = Convert.ToDateTime(value);
                else
                {
                    DateTime valorConvertido;
                    if (DateTime.TryParse(value.ToString(), out valorConvertido))
                        result = valorConvertido;
                }
            }

            return result;
        }


        /// <summary>
        /// Metodo: Converte objeto para TimeSpan.
        /// </summary>
        /// <param name="value">object: valor a ser convertido, podendo ser de qualquer tipo </param>
        /// <returns>TimeSpan: retorna valor convertido, se valor não puder ser convertido retorna null</returns>
        public static TimeSpan? ToTimeSpanOrNull(object value)
        {
            TimeSpan? result = null;

            if (value != null && value != DBNull.Value)
            {
                if (value is TimeSpan)
                    result = TimeSpan.Parse(value.ToString());
                else
                {
                    TimeSpan valorConvertido;
                    if (TimeSpan.TryParse(value.ToString(), out valorConvertido))
                        result = valorConvertido;
                }
            }

            return result;
        }

        /// <summary>
        /// Metodo: Converte objeto para TimeSpan.
        /// </summary>
        /// <param name="value">object: valor a ser convertido, podendo ser de qualquer tipo </param>
        /// <returns>TimeSpan: retorna valor convertido, se valor não puder ser convertido retorna Valor padrão</returns>
        public static TimeSpan ToTimeSpan(object value)
        {
            TimeSpan result = new TimeSpan();

            if (value != null && value != DBNull.Value)
            {
                if (value is TimeSpan)
                    result = TimeSpan.Parse(value.ToString());
                else
                {
                    TimeSpan valorConvertido;
                    if (TimeSpan.TryParse(value.ToString(), out valorConvertido))
                        result = valorConvertido;
                }
            }

            return result;
        }

        /// <summary>
        /// Metodo: Altera formato de decimal que vem da NF-e
        /// </summary>
        /// <param name="valorDecimal">String: Valor que contem na NFe</param>
        /// <returns></returns>
        public static string AlterarFormatoDecimal(string valorDecimal)
        {
            return valorDecimal.Replace(".", ",");
        }

        /// <summary>
        /// Metodo: Converte objeto para char ou seta valor padrão.
        /// </summary>        
        /// <param name="value">char: Valor default para se não for possível converter o valor </param>>
        /// <returns>char: retorna valor convertido, se valor não puder ser convertido retorna valor default</returns>
        public static char ToChar(this object value)
        {
            char result = new char();

            if (value != null && value != DBNull.Value)
            {
                if (value is char)
                    result = Convert.ToChar(value);
                else
                {
                    char valorConvertido;
                    if (char.TryParse(value.ToString(), out valorConvertido))
                        result = valorConvertido;
                }
            }

            return result;
        }

        /// <summary>
        /// Metodo: Converte objeto para char ou seta valor padrão.
        /// </summary>
        /// <param name="value">object: valor a ser convertido, podendo ser de qualquer tipo </param>
        /// <param name="valorDefault">char: Valor default para se não for possível converter o valor </param>>
        /// <returns>char: retorna valor convertido, se valor não puder ser convertido retorna valor default</returns>
        public static char ToChar(object value, char valorDefault)
        {
            char result = valorDefault;

            if (value != null && value != DBNull.Value)
            {
                if (value is char)
                    result = Convert.ToChar(value);
                else
                {
                    char valorConvertido;
                    if (char.TryParse(value.ToString(), out valorConvertido))
                        result = valorConvertido;
                }
            }

            return result;
        }

        /// <summary>
        /// Metodo: Converte objeto para IPAddress
        /// </summary>
        /// <param name="value">object: valor a ser convertido, podendo ser de qualquer tipo </param>        
        /// <returns>IPAddress: retorna valor convertido, se valor não puder ser convertido retorna valor null</returns>
        public static IPAddress ToIpAddress(object value)
        {
            IPAddress result = null;

            if (value != null && value != DBNull.Value)
            {
                if (value is IPAddress)
                    result = IPAddress.Parse(value.ToString());
                else
                {
                    IPAddress valorConvertido;
                    if (IPAddress.TryParse(value.ToString(), out valorConvertido))
                        result = valorConvertido;
                }
            }

            return result;
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToFormatacaoFinanceira(this decimal value)
        {
            CultureInfo portugues = new CultureInfo("pt-BR");
            portugues = (CultureInfo) portugues.Clone();
            portugues.NumberFormat.CurrencyPositivePattern = 2;
            portugues.NumberFormat.CurrencyNegativePattern = 2;

            return value.ToString("C", portugues);
        }

        /// <summary>
        /// Metodo: Converte objeto para double ou seta valor padrão.
        /// </summary>        
        /// <param name="value">char: Valor default para se não for possível converter o valor </param>>
        /// <returns>char: retorna valor convertido, se valor não puder ser convertido retorna valor default</returns>
        public static double ToDouble(this object value)
        {
            double result = new double();

            if (value != null && value != DBNull.Value)
            {
                if (value is double)
                    result = Convert.ToDouble(value);
                else
                {
                    double valorConvertido;
                    if (double.TryParse(value.ToString(), out valorConvertido))
                        result = valorConvertido;
                }
            }

            return result;
        }

        /// <summary>
        /// Metodo: Verifica se object é System.DBNull.Value, caso seja retonar string.Empty caso contrario retorna valor.
        /// </summary>
        /// <param name="value">object:Valor para ser verificado</param>
        /// <returns>object: retornar valor informado caso não seja System.DBNull.Value, se for retorna string.Empty</returns>
        public static object DbNull(object value)
        {
            return (value == DBNull.Value) ? string.Empty : value;
        }

        /// <summary>
        /// Metodo: Converte objeto para float ou seta valor padrão.
        /// </summary>
        /// <param name="value">object:Valor para ser convertido</param>
        /// <returns>float: retorna valor convertido, se valor não puder ser convertido retorna valor default</returns>
        public static float ToFloat(object value)
        {
            float result = new float();

            if (value != null && value != DBNull.Value)
            {
                if (value is float)
                    result = Convert.ToSingle(value);
                else
                {
                    float valorConvertido;
                    if (float.TryParse(value.ToString(), out valorConvertido))
                        result = valorConvertido;
                }
            }

            return result;
        }


        /// <summary>
        ///  
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float? ToFloatOrNull(object value)
        {
            float? result = (float?) null;

            if (value != null && value != DBNull.Value)
            {
                if (value is float)
                    result = Convert.ToSingle(value);
                else
                {
                    float valorConvertido;
                    if (float.TryParse(value.ToString(), out valorConvertido))
                        result = valorConvertido;
                }
            }

            return result;
        }


        /// <summary>
        ///  Formata a string no padrão (000)1234-1234X,  onde o X pode ou não ser informado.
        /// </summary>
        /// <param name="phone"> <see cref="string"/> representando o número do telefone. </param>
        /// <returns> <see cref="string"/> no formato (000)1234-1234X,  onde o X pode ou não ser informado. </returns>
        public static string ToPhoneOrEmpty(this string phone)
        {
            phone = phone.RemoverCaracteresNaoNumericos();

            if ((phone.Length == 10 || phone.Length == 11) && !phone.StartsWith("0"))
            {
                phone = $"0{phone}";
            }

            if (phone.Length == 11 || phone.Length == 12)
            {
                phone = Regex.Replace(phone, @"(\d{3})(\d{4})(\d{4,5})", @"($1)$2-$3");
            }

            if (string.IsNullOrWhiteSpace(phone) || Regex.IsMatch(phone, @"^\((\d| ){3}\)(\d| ){4}-(\d| ){4,5}$"))
            {
                return phone;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        ///  Formata a string no formato de placa
        /// </summary>
        /// <param name="placa"> <see cref="string"/> representacão da placa </param>
        /// <returns> <see cref="string"/> placa formatado, ou no caso de placa inválida string.Empty </returns>
        public static string ToPlacaOrEmpty(this string placa)
        {
            if (placa.IsNullOrWhiteSpace())
                return string.Empty;

            if (placa.ValidarPlaca())
            {
                if (placa.PlacaModeloAntigo() && placa.Length == 7)
                    placa = placa.Insert(3, "-");

                return placa;
            }

            return string.Empty;
        }

        /// <summary>
        ///  Retorna o valor do atributo de um enum.
        ///  Serve apenas para attributos customizados.
        /// </summary>
        /// <typeparam name="T"> Atributo. </typeparam>
        /// <param name="enumValue"> Valor do enum.  </param>
        /// <returns> Valor do atributo. </returns>
        public static T BuscarAtributo<T>(this Enum enumValue) where T : Attribute
        {
            MemberInfo memberInfo = enumValue.GetType().GetMember(enumValue.ToString()).FirstOrDefault();
            if (memberInfo != null)
            {
                T attribute = (T) memberInfo.GetCustomAttributes(typeof(T), false).FirstOrDefault();
                return attribute;
            }

            throw new Exception("O enum não possui o atributo requisitado.");
        }



        /// <summary>
        ///  Clona um objeto
        /// </summary>
        /// <param name="entity"> Objeto a ser clonado. </param>
        /// <typeparam name="TConvert"> Tipo do objeto. </typeparam>
        /// <returns> Clone do objeto original.;</returns>
        public static TConvert Clone<TConvert>(this object entity) where TConvert : new()
        {
            var convert = new TConvert();
            CopyTo(entity, convert);

            return convert;
        }

        private static void CopyTo(this object s, object T)
        {
            foreach (var pS in s.GetType().GetProperties())
            {
                foreach (var pT in T.GetType().GetProperties())
                {
                    if (pT.Name != pS.Name) continue;
                    (pT.GetSetMethod()).Invoke(T, new object[] {pS.GetGetMethod().Invoke(s, null)});
                }
            }
        }

        /// <summary>
        ///  Converte um decimal para seu valor por extenso em reais.
        ///  Código baseado no exemplo postado em https://ivanmeirelles.wordpress.com/2012/10/27/escrever-valores-por-extenso-em-c/
        /// </summary>
        /// <param name="valor"> Decimal: Valor que será convertido. </param>
        /// <returns> String: Valor por extenso em reais. </returns>
        public static string ToValorPorExtenso(this decimal valor)
        {
            if (valor <= 0 | valor >= 1000000000000000)
                return "Valor não suportado pelo sistema.";
            else
            {
                string strValor = valor.ToString("000000000000000.00");
                string valorPorExtenso = string.Empty;

                for (int i = 0; i <= 15; i += 3)
                {
                    valorPorExtenso += CentenaDezenaUnidaePorEstenso(Convert.ToDecimal(strValor.Substring(i, 3)));
                    if (i == 0 & valorPorExtenso != string.Empty)
                    {
                        if (Convert.ToInt32(strValor.Substring(0, 3)) == 1)
                            valorPorExtenso +=
                                $" TRILHÃO{((Convert.ToDecimal(strValor.Substring(3, 12)) > 0) ? " E " : string.Empty)}";
                        else if (Convert.ToInt32(strValor.Substring(0, 3)) > 1)
                            valorPorExtenso +=
                                $" TRILHÕES{((Convert.ToDecimal(strValor.Substring(3, 12)) > 0) ? " E " : string.Empty)}";
                    }
                    else if (i == 3 & valorPorExtenso != string.Empty)
                    {
                        if (Convert.ToInt32(strValor.Substring(3, 3)) == 1)
                            valorPorExtenso +=
                                $" BILHÃO{((Convert.ToDecimal(strValor.Substring(6, 9)) > 0) ? " E " : string.Empty)}";
                        else if (Convert.ToInt32(strValor.Substring(3, 3)) > 1)
                            valorPorExtenso +=
                                $" BILHÕES{((Convert.ToDecimal(strValor.Substring(6, 9)) > 0) ? " E " : string.Empty)}";
                    }
                    else if (i == 6 & valorPorExtenso != string.Empty)
                    {
                        if (Convert.ToInt32(strValor.Substring(6, 3)) == 1)
                            valorPorExtenso +=
                                $" MILHÃO{((Convert.ToDecimal(strValor.Substring(9, 6)) > 0) ? " E " : string.Empty)}";
                        else if (Convert.ToInt32(strValor.Substring(6, 3)) > 1)
                            valorPorExtenso +=
                                $" MILHÕES{((Convert.ToDecimal(strValor.Substring(9, 6)) > 0) ? " E " : string.Empty)}";
                    }
                    else if (i == 9 & valorPorExtenso != string.Empty)
                        if (Convert.ToInt32(strValor.Substring(9, 3)) > 0)
                            valorPorExtenso +=
                                $" MIL{((Convert.ToDecimal(strValor.Substring(12, 3)) > 0) ? " E " : string.Empty)}";

                    if (i == 12)
                    {
                        if (valorPorExtenso.Length > 8)
                            if (valorPorExtenso.Substring(valorPorExtenso.Length - 6, 6) == "BILHÃO" |
                                valorPorExtenso.Substring(valorPorExtenso.Length - 6, 6) == "MILHÃO")
                                valorPorExtenso += " DE";
                            else if (valorPorExtenso.Substring(valorPorExtenso.Length - 7, 7) == "BILHÕES" |
                                     valorPorExtenso.Substring(valorPorExtenso.Length - 7, 7) == "MILHÕES" |
                                     valorPorExtenso.Substring(valorPorExtenso.Length - 8, 7) == "TRILHÕES")
                                valorPorExtenso += " DE";
                            else if (valorPorExtenso.Substring(valorPorExtenso.Length - 8, 8) == "TRILHÕES")
                                valorPorExtenso += " DE";

                        if (Convert.ToInt64(strValor.Substring(0, 15)) == 1)
                            valorPorExtenso += " REAL";
                        else if (Convert.ToInt64(strValor.Substring(0, 15)) > 1)
                            valorPorExtenso += " REAIS";

                        if (Convert.ToInt32(strValor.Substring(16, 2)) > 0 && valorPorExtenso != string.Empty)
                            valorPorExtenso += " E ";
                    }

                    if (i == 15)
                        if (Convert.ToInt32(strValor.Substring(16, 2)) == 1)
                            valorPorExtenso += " CENTAVO";
                        else if (Convert.ToInt32(strValor.Substring(16, 2)) > 1)
                            valorPorExtenso += " CENTAVOS";
                }

                return valorPorExtenso;
            }
        }

        private static string CentenaDezenaUnidaePorEstenso(decimal valor)
        {
            if (valor <= 0)
                return string.Empty;
            else
            {
                string montagem = string.Empty;
                if (valor > 0 & valor < 1)
                {
                    valor *= 100;
                }

                string strValor = valor.ToString("000");
                int a = Convert.ToInt32(strValor.Substring(0, 1));
                int b = Convert.ToInt32(strValor.Substring(1, 1));
                int c = Convert.ToInt32(strValor.Substring(2, 1));

                if (a == 1) montagem += (b + c == 0) ? "CEM" : "CENTO";
                else if (a == 2) montagem += "DUZENTOS";
                else if (a == 3) montagem += "TREZENTOS";
                else if (a == 4) montagem += "QUATROCENTOS";
                else if (a == 5) montagem += "QUINHENTOS";
                else if (a == 6) montagem += "SEISCENTOS";
                else if (a == 7) montagem += "SETECENTOS";
                else if (a == 8) montagem += "OITOCENTOS";
                else if (a == 9) montagem += "NOVECENTOS";

                if (b == 1)
                {
                    if (c == 0) montagem += $"{((a > 0) ? " E " : string.Empty)}DEZ";
                    else if (c == 1) montagem += $"{((a > 0) ? " E " : string.Empty)}ONZE";
                    else if (c == 2) montagem += $"{((a > 0) ? " E " : string.Empty)}DOZE";
                    else if (c == 3) montagem += $"{((a > 0) ? " E " : string.Empty)}TREZE";
                    else if (c == 4) montagem += $"{((a > 0) ? " E " : string.Empty)}QUATORZE";
                    else if (c == 5) montagem += $"{((a > 0) ? " E " : string.Empty)}QUINZE";
                    else if (c == 6) montagem += $"{((a > 0) ? " E " : string.Empty)}DEZESSEIS";
                    else if (c == 7) montagem += $"{((a > 0) ? " E " : string.Empty)}DEZESSETE";
                    else if (c == 8) montagem += $"{((a > 0) ? " E " : string.Empty)}DEZOITO";
                    else if (c == 9) montagem += $"{((a > 0) ? " E " : string.Empty)}DEZENOVE";
                }
                else if (b == 2) montagem += $"{((a > 0) ? " E " : string.Empty)}VINTE";
                else if (b == 3) montagem += $"{((a > 0) ? " E " : string.Empty)}TRINTA";
                else if (b == 4) montagem += $"{((a > 0) ? " E " : string.Empty)}QUARENTA";
                else if (b == 5) montagem += $"{((a > 0) ? " E " : string.Empty)}CINQUENTA";
                else if (b == 6) montagem += $"{((a > 0) ? " E " : string.Empty)}SESSENTA";
                else if (b == 7) montagem += $"{((a > 0) ? " E " : string.Empty)}SETENTA";
                else if (b == 8) montagem += $"{((a > 0) ? " E " : string.Empty)}OITENTA";
                else if (b == 9) montagem += $"{((a > 0) ? " E " : string.Empty)}NOVENTA";

                if (strValor.Substring(1, 1) != "1" & c != 0 & montagem != string.Empty) montagem += " E ";

                if (strValor.Substring(1, 1) != "1")
                    if (c == 1)
                        montagem += "UM";
                    else if (c == 2) montagem += "DOIS";
                    else if (c == 3) montagem += "TRÊS";
                    else if (c == 4) montagem += "QUATRO";
                    else if (c == 5) montagem += "CINCO";
                    else if (c == 6) montagem += "SEIS";
                    else if (c == 7) montagem += "SETE";
                    else if (c == 8) montagem += "OITO";
                    else if (c == 9) montagem += "NOVE";

                return montagem;
            }
        }

        /// <summary>
        ///  Verifica se a string é uma string base 64 válida.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsBase64String(this string s)
        {
            s = s.Trim();
            return (s.Length % 4 == 0) && Regex.IsMatch(s, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);
        }

        /// <summary>
        ///  Retorna a versão formatada no padrão v.vv.vv
        /// </summary>
        /// <param name="versao"></param>
        /// <returns></returns>
        public static string VersaoFormatada(long versao)
        {
            var v = versao.ToString().FormatarString(6, EDirecao.Esquerda, '0');
            return $"{v.Substring(0, 2).ToInt()}.{v.Substring(2, 2)}.{v.Substring(4, 2)}";
        }

        /// <summary>
        ///  Transforma uma quantidade de bytes na maior unidade possivel.
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ToByteFormatedSize(this long bytes)
        {
            decimal b = bytes;

            string[] sufix = {"B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB"};
            int index = 0;
            do
            {
                b /= 1024;
                index++;
            } while (b >= 1024);

            return $"{b:0.00} {sufix[index]}";
        }

        /// <summary>
        ///  Retorna uma lista de strings representando o texto.
        /// </summary>
        /// <param name="text"> Texto. </param>
        /// <returns> Lista de strings representando o texto. </returns>
        public static LinkedList<string> Lines(this StringReader text)
        {
            string line;
            var lines = new LinkedList<string>();

            while ((line = text.ReadLine()) != null)
            {
                lines.AddLast(line);
            }

            return lines;
        }

        /// <summary>
        ///  Retorna uma lista de strings representando o texto.
        /// </summary>
        /// <param name="text"> Texto. </param>
        /// <returns> Lista de strings representando o texto. </returns>
        public static LinkedList<string> Lines(this string text)
        {
            string line;
            var lines = new LinkedList<string>();

            var stringReader = new StringReader(text);

            while ((line = stringReader.ReadLine()) != null)
            {
                lines.AddLast(line);
            }

            return lines;
        }

    }
}
