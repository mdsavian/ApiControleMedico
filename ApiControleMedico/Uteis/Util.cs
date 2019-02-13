using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using ApiControleMedico.Modelos.Enums;
using ApiControleMedico.Uteis;


namespace ApiControleMedico.Uteis
{
    /// <summary>
    /// 
    /// </summary>
    public static class Util
    {
        
        /// <summary>
        ///  Formata a string para que tenha um tamanho máximo.
        ///  Caso o tamanho inicial seja inferior ao tamanho informado a string irá crescer em uma direção sendo preenchida com o caracter informado.
        /// </summary>
        /// <param name="texto"> Texto inicial. </param>
        /// <param name="tamanho"> Tamanho máximo da string. </param>
        /// <param name="direcao"> Direção para onde a string deve crescer. </param>
        /// <param name="caracter"> Caracter que ira preencer a string. Caso não informado a string será preenchida com espaços em branco. </param>
        /// <returns> String preenchida com o caracter selecionado. </returns>
        public static string FormatarString(this string texto, int tamanho, EDirecao direcao = EDirecao.Centro, char caracter = ' ')
        {
            if (texto.Length == tamanho)
            {
                return texto;
            }
            else if (texto.Length < tamanho)
            {
                if (direcao == EDirecao.Direita)
                {
                    var quantidadeCaracteres = tamanho - texto.Length;
                    return $"{texto}{new string(caracter, quantidadeCaracteres)}";
                }
                else if (direcao == EDirecao.Esquerda)
                {
                    var quantidadeCaracteres = tamanho - texto.Length;
                    return $"{new string(caracter, quantidadeCaracteres)}{texto}";
                }
                else if (direcao == EDirecao.Centro)
                {
                    var direcaoParaAdicionarCaracter = EDirecao.Direita;
                    var textoCentralizado = texto;
                    while (textoCentralizado.Length < tamanho)
                    {
                        if (direcaoParaAdicionarCaracter == EDirecao.Direita)
                        {
                            textoCentralizado = $"{textoCentralizado}{caracter}";
                            direcaoParaAdicionarCaracter = EDirecao.Esquerda;
                        }
                        else
                        {
                            textoCentralizado = $"{caracter}{textoCentralizado}";
                            direcaoParaAdicionarCaracter = EDirecao.Direita;
                        }
                    }

                    return textoCentralizado;
                }
            }
            else
            {
                return texto.Substring(0, tamanho);
            }

            return texto;
        }

        /// <summary>
        ///  Remove os caractéres não numéricos da string.
        /// </summary>
        /// <param name="s"> String de entrada. </param>
        /// <returns> String contendo apenas caractéres numéricos. </returns>
        public static string RemoverCaracteresNaoNumericos(this string s)
        {
            return string.IsNullOrWhiteSpace(s) ? s : Regex.Replace(s, "[^0-9]", "");
        }

        public static string RemoverTags(this string s)
        {
            return string.IsNullOrWhiteSpace(s) ? s : s.Replace("<b>", "").Replace("</b>", "").Replace("<e>", "").Replace("</e>", "");
        }

        /// <summary>
        ///  Remove os caractéres não alfanuméricos da string.
        /// </summary>
        /// <param name="s"> String de entrada. </param>
        /// <returns> String contendo apenas caractéres alfanuméricos. </returns>
        public static string RemoverCaracteresEspeciais(this string s)
        {
            return string.IsNullOrWhiteSpace(s) ? s : Regex.Replace(s, "[^a-zA-Z0-9]", "");
        }

        /// <summary>
        ///  Varifica se uma string contém letras.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool ContemLetras(string s)
        {
            if (s.IsNullOrWhiteSpace())
                return false;

            return Regex.IsMatch(s, @"^[a-zA-Z]+$");
        }

        /// <summary>
        ///  Verifica se um texto contém outro texto. 
        /// </summary>
        /// <param name="source"> String: texto. </param>
        /// <param name="toCheck"> String: trecho que será buscado dentro do texto. </param>
        /// <param name="comparison"> <see cref="StringComparison"/> </param>
        /// <returns></returns>
        public static bool Contains(this string source, string toCheck, StringComparison comparison)
        {
            return source.IndexOf(toCheck, comparison) >= 0;
        }

        /// <summary>
        ///  Verifica se a string informada representa um CPF válido.
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns></returns>
        public static bool ValidarCpf(this string cpf)
        {
            var caracteresCpf = cpf.RemoverCaracteresNaoNumericos();

            if (caracteresCpf.Length != 11)
            {
                return false;
            }

            if ((caracteresCpf.Equals(new string('0', 11))) || (caracteresCpf.Equals(new string('1', 11))) ||
                (caracteresCpf.Equals(new string('2', 11))) || (caracteresCpf.Equals(new string('3', 11))) ||
                (caracteresCpf.Equals(new string('4', 11))) || (caracteresCpf.Equals(new string('5', 11))) ||
                (caracteresCpf.Equals(new string('6', 11))) || (caracteresCpf.Equals(new string('7', 11))) ||
                (caracteresCpf.Equals(new string('8', 11))) || (caracteresCpf.Equals(new string('9', 11))))
            {
                return false;
            }

            int primeiroDigVerificador = 0;
            int segundoDigVerificador = 0;
            int primeiroMultiplicador = 10;
            int segundoMultiplicador = 11;

            for (var i = 0; i < 9; i++)
            {
                primeiroDigVerificador += int.Parse(caracteresCpf[i].ToString()) * primeiroMultiplicador;
                primeiroMultiplicador -= 1;
            }

            for (var i = 0; i < 10; i++)
            {
                segundoDigVerificador += int.Parse(caracteresCpf[i].ToString()) * segundoMultiplicador;
                segundoMultiplicador -= 1;
            }

            primeiroDigVerificador = ((primeiroDigVerificador * 10) % 11 == 10) ? 0 : ((primeiroDigVerificador * 10) % 11);
            segundoDigVerificador = ((segundoDigVerificador * 10) % 11 == 10) ? 0 : ((segundoDigVerificador * 10) % 11);

            return caracteresCpf[9].ToInt() == primeiroDigVerificador && caracteresCpf[10].ToInt() == segundoDigVerificador;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="placa"></param>
        /// <returns></returns>
        public static bool PlacaModeloAntigo(this string placa)
        {
            int numeros = 0;
            int letras = 0;

            //valida pelo formato padrão de placas.
            if (placa.Length == 7)
            {
                if (placa.IndexOf("-") == -1)
                    placa = placa.Insert(3, "-");
            }

            if (placa.Length == 8)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (char.IsLetter(placa[i]))
                        letras++;
                }
                for (int i = 4; i < 8; i++)
                {
                    if (char.IsNumber(placa[i]))
                        numeros++;
                }
            }

            return ((letras == 3) && (numeros == 4));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="placa"></param>
        /// <returns></returns>
        public static bool ValidarPlaca(this string placa)
        {
            var result = true;
            var placaAntiga = placa;
            int numeros = 0;
            int letras = 0;

            //valida pelo formato padrão de placas.
            if (placaAntiga.Length == 7)
            {
                if (placaAntiga.IndexOf("-") == -1)
                    placaAntiga = placaAntiga.Insert(3, "-");
            }

            if (placaAntiga.Length == 8)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (char.IsLetter(placaAntiga[i]))
                        letras++;
                }
                for (int i = 4; i < 8; i++)
                {
                    if (char.IsNumber(placaAntiga[i]))
                        numeros++;
                }
            }

            result = ((letras == 3) && ((numeros == 4) || numeros == 3 && placaAntiga.Contains("+")) && placaAntiga.Contains("-")) || string.IsNullOrEmpty(placa);

            //case formato seja inválido, validará pelo novo formato de placas.
            if (!result)
            {
                if (placa.Length == 7)
                {
                    numeros = 0;
                    letras = 0;

                    for (int i = 0; i < 7; i++)
                    {
                        if (char.IsLetter(placa[i]))
                            letras++;
                        else if (char.IsNumber(placa[i]))
                            numeros++;
                    }

                    result = ((letras == 4) && (numeros == 3));
                }
            }

            return result;
        }

        /// <summary>
        ///  Verifica se uma string representa um CNPJ válido.
        /// </summary>
        /// <param name="cnpj"></param>
        /// <returns></returns>
        public static bool ValidarCnpj(this string cnpj)
        {
            var caracteresCnpj = cnpj.RemoverCaracteresNaoNumericos();

            // tamanho inferior a 14 ou quando todos os digitos são iguais
            if (caracteresCnpj.Length != 14 || caracteresCnpj.Distinct().Count() == 1)
            {
                return false;
            }

            int primeiroDigitoVerificador = 0, segundoDigitoVerificador = 0;

            for (var i = 5; i <= 6; i++)
            {
                var soma = 0;
                var flag = i;

                for (var c = 0; c < (i + 7); c++)
                {
                    soma += caracteresCnpj[c].ToInt() * flag;
                    flag = (flag > 2) ? flag - 1 : 9;
                }

                soma = soma % 11;

                if (i == 5) primeiroDigitoVerificador = (soma > 1) ? 11 - soma : 0;
                if (i == 6) segundoDigitoVerificador = (soma > 1) ? 11 - soma : 0;

            }

            return primeiroDigitoVerificador == caracteresCnpj[12].ToInt() && segundoDigitoVerificador == caracteresCnpj[13].ToInt();
        }

        /// <summary>
        ///  Verifica se o tipo especificado é um Nullable Enum.
        /// </summary>
        /// <param name="t"> <see cref="Type"/> </param>
        /// <returns> True caso o tipo especificado seja um Nullable Enum ou false caso contrário. </returns>
        public static bool IsNullableEnum(this Type t)
        {
            Type u = Nullable.GetUnderlyingType(t);
            return (u != null) && u.IsEnum;
        }

        /// <summary>
        ///  Transforma a primeira letra de cada palvra da string em maiúscula.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToTitleCase(this string str)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
        }

        /// <summary>
        ///  Verifica se a enumeração é diferente de nula e possui itens.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <returns> True caso e enumeração possua itens válidos, false caso contrário. </returns>
        public static bool HasItems<T>(this IEnumerable<T> enumerable)
        {
            return enumerable != null && enumerable.Any();
        }

        /// <summary>
        ///  Verifica se o <see cref="DataTable"/> é diferente de null e posui uma ou mais linhas.
        /// </summary>
        /// <param name="dataTable"> <see cref="DataTable"/>. </param>
        /// <returns> <see cref="bool"/> true caso a <see cref="DataTable"/> contenha registros e não seja nula, caso contrário false. </returns>
        public static bool HasRows(this DataTable dataTable)
        {
            return (dataTable != null && dataTable.Rows.Any());
        }

        /// <summary>
        ///  Verifica se um data está entre outras duas datas, incluíndo as próprias datas inicial e final.
        /// </summary>
        /// <param name="data"> <see cref="DateTime"/> Data que será validada </param>
        /// <param name="dataInicial"> <see cref="DateTime"/> Data inicial. </param>
        /// <param name="dataFinal"> <see cref="DateTime"/> Data final. </param>
        /// <returns> bool: True caso a data solicitada esteja entre as outras, false caso contrário. </returns>
        public static bool Between(this DateTime data, DateTime dataInicial, DateTime dataFinal)
        {
            return (data.Date >= dataInicial.Date && data.Date <= dataFinal.Date);
        }

        /// <summary>
        ///  Indica se existem linhas no <see cref="DataRowCollection"/>.
        /// </summary>
        /// <param name="dataRowCollection"> <see cref="DataRowCollection"/>. </param>
        /// <returns> <see cref="bool"/> true caso a <see cref="DataRowCollection"/> contenha registros e não seja nula, caso contrário false. </returns>
        public static bool Any(this DataRowCollection dataRowCollection)
        {
            return (dataRowCollection != null && dataRowCollection.Count > 0);
        }

        /// <summary>
        ///  Valida uma inscrição estadual.
        /// </summary>
        /// <param name="uf"> Sigla da UF. </param>
        /// <param name="inscricaoEstadual"> Inscrição Estadual. </param>
        /// <returns></returns>
        public static bool ValidarInscricaoEstadual(string uf, string inscricaoEstadual)
        {
            try
            {
                bool retorno = false;
                string strBase;
                string strBase2;
                string strOrigem;
                string strDigito1;
                string strDigito2;
                int intPos;
                int intValor;
                int intSoma = 0;
                int intResto;
                int intNumero;
                int intPeso = 0;

                strBase = "";
                strBase2 = "";
                strOrigem = "";

                if ((inscricaoEstadual.Trim().ToUpper() == "ISENTO"))
                    return true;

                for (intPos = 1; intPos <= inscricaoEstadual.Trim().Length; intPos++)
                {
                    if ((("0123456789P".IndexOf(inscricaoEstadual.Substring((intPos - 1), 1), 0, StringComparison.OrdinalIgnoreCase) + 1) > 0))
                        strOrigem = (strOrigem + inscricaoEstadual.Substring((intPos - 1), 1));
                }

                switch (uf.ToUpper())
                {
                    case "AC":

                        #region

                        strBase = (strOrigem.Trim() + "00000000000").Substring(0, 11);

                        if (strBase.Substring(0, 2) == "01")
                        {
                            intSoma = 0;
                            intPeso = 4;

                            for (intPos = 1; (intPos <= 11); intPos++)
                            {
                                intValor = int.Parse(strBase.Substring((intPos - 1), 1));

                                if (intPeso == 1) intPeso = 9;

                                intSoma += intValor * intPeso;

                                intPeso--;
                            }

                            intResto = (intSoma % 11);
                            strDigito1 = ((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Substring((((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Length - 1));

                            intSoma = 0;
                            strBase = (strOrigem.Trim() + "000000000000").Substring(0, 12);
                            intPeso = 5;

                            for (intPos = 1; (intPos <= 12); intPos++)
                            {
                                intValor = int.Parse(strBase.Substring((intPos - 1), 1));

                                if (intPeso == 1) intPeso = 9;

                                intSoma += intValor * intPeso;
                                intPeso--;
                            }

                            intResto = (intSoma % 11);
                            strDigito2 = ((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Substring((((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Length - 1));

                            strBase2 = (strBase.Substring(0, 12) + strDigito2);

                            if ((strBase2 == strOrigem))
                                retorno = true;
                        }

                        #endregion

                        break;

                    case "AL":

                        #region

                        strBase = (strOrigem.Trim() + "000000000").Substring(0, 9);

                        if ((strBase.Substring(0, 2) == "24"))
                        {
                            intSoma = 0;
                            intPeso = 9;

                            for (intPos = 1; (intPos <= 8); intPos++)
                            {
                                intValor = int.Parse(strBase.Substring((intPos - 1), 1));

                                intSoma += intValor * intPeso;
                                intPeso--;
                            }

                            intSoma = (intSoma * 10);
                            intResto = (intSoma % 11);

                            strDigito1 = ((intResto == 10) ? "0" : Convert.ToString(intResto)).Substring((((intResto == 10) ? "0" : Convert.ToString(intResto)).Length - 1));

                            strBase2 = (strBase.Substring(0, 8) + strDigito1);

                            if ((strBase2 == strOrigem))
                                retorno = true;
                        }

                        #endregion

                        break;

                    case "AM":

                        #region

                        strBase = (strOrigem.Trim() + "000000000").Substring(0, 9);
                        intSoma = 0;
                        intPeso = 9;

                        for (intPos = 1; (intPos <= 8); intPos++)
                        {
                            intValor = int.Parse(strBase.Substring((intPos - 1), 1));

                            intSoma += intValor * intPeso;
                            intPeso--;
                        }

                        intResto = (intSoma % 11);

                        if (intSoma < 11)
                            strDigito1 = (11 - intSoma).ToString();
                        else
                            strDigito1 = ((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Substring((((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Length - 1));

                        strBase2 = (strBase.Substring(0, 8) + strDigito1);

                        if ((strBase2 == strOrigem))
                            retorno = true;

                        #endregion

                        break;

                    case "AP":

                        #region

                        strBase = (strOrigem.Trim() + "000000000").Substring(0, 9);
                        intPeso = 9;

                        if ((strBase.Substring(0, 2) == "03"))
                        {
                            strBase = (strOrigem.Trim() + "000000000").Substring(0, 9);
                            intSoma = 0;

                            for (intPos = 1; (intPos <= 8); intPos++)
                            {
                                intValor = int.Parse(strBase.Substring((intPos - 1), 1));

                                intSoma += intValor * intPeso;
                                intPeso--;
                            }

                            intResto = (intSoma % 11);
                            intValor = (11 - intResto);

                            strDigito1 = Convert.ToString(intValor).Substring((Convert.ToString(intValor).Length - 1));

                            strBase2 = (strBase.Substring(0, 8) + strDigito1);

                            if ((strBase2 == strOrigem))
                                retorno = true;
                        }

                        #endregion

                        break;

                    case "BA":

                        #region

                        if (strOrigem.Length == 8)
                            strBase = (strOrigem.Trim() + "00000000").Substring(0, 8);
                        else if (strOrigem.Length == 9)
                            strBase = (strOrigem.Trim() + "00000000").Substring(0, 9);

                        if ((("0123458".IndexOf(strBase.Substring(0, 1), 0, System.StringComparison.OrdinalIgnoreCase) + 1) > 0) && strBase.Length == 8)
                        {
                            #region

                            intSoma = 0;

                            for (intPos = 1; (intPos <= 6); intPos++)
                            {
                                intValor = int.Parse(strBase.Substring((intPos - 1), 1));

                                if (intPos == 1) intPeso = 7;

                                intSoma += intValor * intPeso;
                                intPeso--;
                            }


                            intResto = (intSoma % 10);
                            strDigito2 = ((intResto == 0) ? "0" : Convert.ToString((10 - intResto))).Substring((((intResto == 0) ? "0" : Convert.ToString((10 - intResto))).Length - 1));


                            strBase2 = strBase.Substring(0, 7) + strDigito2;

                            if (strBase2 == strOrigem)
                                retorno = true;

                            if (retorno)
                            {
                                intSoma = 0;
                                intPeso = 0;

                                for (intPos = 1; (intPos <= 7); intPos++)
                                {
                                    intValor = int.Parse(strBase.Substring((intPos - 1), 1));

                                    if (intPos == 7)
                                        intValor = int.Parse(strBase.Substring((intPos), 1));

                                    if (intPos == 1) intPeso = 8;

                                    intSoma += intValor * intPeso;
                                    intPeso--;
                                }


                                intResto = (intSoma % 10);
                                strDigito1 = ((intResto == 0) ? "0" : Convert.ToString((10 - intResto))).Substring((((intResto == 0) ? "0" : Convert.ToString((10 - intResto))).Length - 1));

                                strBase2 = (strBase.Substring(0, 6) + strDigito1 + strDigito2);

                                if ((strBase2 == strOrigem))
                                    retorno = true;
                            }

                            #endregion
                        }
                        else if ((("679".IndexOf(strBase.Substring(0, 1), 0, System.StringComparison.OrdinalIgnoreCase) + 1) > 0) && strBase.Length == 8)
                        {
                            #region

                            intSoma = 0;

                            for (intPos = 1; (intPos <= 6); intPos++)
                            {
                                intValor = int.Parse(strBase.Substring((intPos - 1), 1));

                                if (intPos == 1) intPeso = 7;

                                intSoma += intValor * intPeso;
                                intPeso--;
                            }


                            intResto = (intSoma % 11);
                            strDigito2 = ((intResto == 0) ? "0" : Convert.ToString((11 - intResto))).Substring((((intResto == 0) ? "0" : Convert.ToString((11 - intResto))).Length - 1));


                            strBase2 = strBase.Substring(0, 7) + strDigito2;

                            if (strBase2 == strOrigem)
                                retorno = true;

                            if (retorno)
                            {
                                intSoma = 0;
                                intPeso = 0;

                                for (intPos = 1; (intPos <= 7); intPos++)
                                {
                                    intValor = int.Parse(strBase.Substring((intPos - 1), 1));

                                    if (intPos == 7)
                                        intValor = int.Parse(strBase.Substring((intPos), 1));

                                    if (intPos == 1) intPeso = 8;

                                    intSoma += intValor * intPeso;
                                    intPeso--;
                                }


                                intResto = (intSoma % 11);
                                strDigito1 = ((intResto == 0) ? "0" : Convert.ToString((11 - intResto))).Substring((((intResto == 0) ? "0" : Convert.ToString((11 - intResto))).Length - 1));

                                strBase2 = (strBase.Substring(0, 6) + strDigito1 + strDigito2);

                                if ((strBase2 == strOrigem))
                                    retorno = true;
                            }

                            #endregion
                        }
                        else if ((("0123458".IndexOf(strBase.Substring(1, 1), 0, System.StringComparison.OrdinalIgnoreCase) + 1) > 0) && strBase.Length == 9)
                        {
                            #region

                            /* Segundo digito */
                            //1000003
                            //8765432
                            intSoma = 0;


                            for (intPos = 1; (intPos <= 7); intPos++)
                            {
                                intValor = int.Parse(strBase.Substring((intPos - 1), 1));

                                if (intPos == 1) intPeso = 8;

                                intSoma += intValor * intPeso;
                                intPeso--;
                            }

                            intResto = (intSoma % 10);
                            strDigito2 = ((intResto == 0) ? "0" : Convert.ToString((10 - intResto))).Substring((((intResto == 0) ? "0" : Convert.ToString((10 - intResto))).Length - 1));

                            strBase2 = strBase.Substring(0, 8) + strDigito2;

                            if (strBase2 == strOrigem)
                                retorno = true;

                            if (retorno)
                            {
                                //1000003 6
                                //9876543 2
                                intSoma = 0;
                                intPeso = 0;

                                for (intPos = 1; (intPos <= 8); intPos++)
                                {
                                    intValor = int.Parse(strBase.Substring((intPos - 1), 1));

                                    if (intPos == 8)
                                        intValor = int.Parse(strBase.Substring((intPos), 1));

                                    if (intPos == 1) intPeso = 9;

                                    intSoma += intValor * intPeso;
                                    intPeso--;
                                }


                                intResto = (intSoma % 10);
                                strDigito1 = ((intResto == 0) ? "0" : Convert.ToString((11 - intResto))).Substring((((intResto == 0) ? "0" : Convert.ToString((11 - intResto))).Length - 1));

                                strBase2 = (strBase.Substring(0, 7) + strDigito1 + strDigito2);

                                if ((strBase2 == strOrigem))
                                    retorno = true;
                            }

                            #endregion
                        }

                        #endregion

                        break;

                    case "CE":

                        #region

                        strBase = (strOrigem.Trim() + "000000000").Substring(0, 9);
                        intSoma = 0;

                        for (intPos = 1; (intPos <= 8); intPos++)
                        {
                            intValor = int.Parse(strBase.Substring((intPos - 1), 1));
                            intValor = (intValor * (10 - intPos));
                            intSoma = (intSoma + intValor);
                        }

                        intResto = (intSoma % 11);
                        intValor = (11 - intResto);

                        if ((intValor > 9))
                            intValor = 0;

                        strDigito1 = Convert.ToString(intValor).Substring((Convert.ToString(intValor).Length - 1));

                        strBase2 = (strBase.Substring(0, 8) + strDigito1);

                        if ((strBase2 == strOrigem))
                            retorno = true;

                        #endregion

                        break;

                    case "DF":

                        #region

                        strBase = $"{strOrigem.Trim()}0000000000000".Substring(0, 13);

                        intSoma = 0;
                        intPeso = 2;

                        for (intPos = 11; (intPos >= 1); intPos = (intPos + -1))
                        {
                            intValor = int.Parse(strBase.Substring((intPos - 1), 1));
                            intValor = (intValor * intPeso);
                            intSoma = (intSoma + intValor);
                            intPeso = (intPeso + 1);

                            if ((intPeso > 9))
                                intPeso = 2;
                        }

                        intResto = (intSoma % 11);
                        strDigito1 = ((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Substring((((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Length - 1));
                        strBase2 = (strBase.Substring(0, 11) + strDigito1);
                        intSoma = 0;
                        intPeso = 2;

                        for (intPos = 12; (intPos >= 1); intPos = (intPos + -1))
                        {
                            intValor = int.Parse(strBase.Substring((intPos - 1), 1));
                            intValor = (intValor * intPeso);
                            intSoma = (intSoma + intValor);
                            intPeso = (intPeso + 1);

                            if ((intPeso > 9))
                                intPeso = 2;
                        }

                        intResto = (intSoma % 11);
                        strDigito2 = ((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Substring((((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Length - 1));
                        strBase2 = (strBase.Substring(0, 12) + strDigito2);

                        if ((strBase2 == strOrigem))
                            retorno = true;

                        #endregion

                        break;

                    case "ES":

                        #region

                        strBase = (strOrigem.Trim() + "000000000").Substring(0, 9);
                        intSoma = 0;

                        for (intPos = 1; (intPos <= 8); intPos++)
                        {
                            intValor = int.Parse(strBase.Substring((intPos - 1), 1));
                            intValor = (intValor * (10 - intPos));
                            intSoma = (intSoma + intValor);
                        }

                        intResto = (intSoma % 11);
                        strDigito1 = ((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Substring((((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Length - 1));
                        strBase2 = (strBase.Substring(0, 8) + strDigito1);

                        if ((strBase2 == strOrigem))
                            retorno = true;

                        #endregion

                        break;

                    case "GO":

                        #region

                        strBase = (strOrigem.Trim() + "000000000").Substring(0, 9);

                        if ((("10,11,15".IndexOf(strBase.Substring(0, 2), 0, System.StringComparison.OrdinalIgnoreCase) + 1) > 0))
                        {
                            intSoma = 0;

                            for (intPos = 1; (intPos <= 8); intPos++)
                            {
                                intValor = int.Parse(strBase.Substring((intPos - 1), 1));
                                intValor = (intValor * (10 - intPos));
                                intSoma = (intSoma + intValor);
                            }

                            intResto = (intSoma % 11);

                            if ((intResto == 0))
                                strDigito1 = "0";
                            else if ((intResto == 1))
                            {
                                intNumero = int.Parse(strBase.Substring(0, 8));
                                strDigito1 = (((intNumero >= 10103105) && (intNumero <= 10119997)) ? "1" : "0").Substring(((((intNumero >= 10103105) && (intNumero <= 10119997)) ? "1" : "0").Length - 1));
                            }
                            else
                                strDigito1 = Convert.ToString((11 - intResto)).Substring((Convert.ToString((11 - intResto)).Length - 1));

                            strBase2 = (strBase.Substring(0, 8) + strDigito1);

                            if ((strBase2 == strOrigem))
                                retorno = true;
                        }

                        #endregion

                        break;

                    case "MA":

                        #region

                        strBase = (strOrigem.Trim() + "000000000").Substring(0, 9);

                        if ((strBase.Substring(0, 2) == "12"))
                        {
                            intSoma = 0;

                            for (intPos = 1; (intPos <= 8); intPos++)
                            {
                                intValor = int.Parse(strBase.Substring((intPos - 1), 1));
                                intValor = (intValor * (10 - intPos));
                                intSoma = (intSoma + intValor);
                            }

                            intResto = (intSoma % 11);
                            strDigito1 = ((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Substring((((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Length - 1));
                            strBase2 = (strBase.Substring(0, 8) + strDigito1);

                            if ((strBase2 == strOrigem))
                                retorno = true;
                        }

                        #endregion

                        break;

                    case "MT":

                        #region

                        strBase = (strOrigem.Trim() + "0000000000").Substring(0, 10);
                        intSoma = 0;
                        intPeso = 2;

                        for (intPos = 10; intPos >= 1; intPos = (intPos + -1))
                        {
                            intValor = int.Parse(strBase.Substring((intPos - 1), 1));
                            intValor = (intValor * intPeso);
                            intSoma = (intSoma + intValor);
                            intPeso = (intPeso + 1);

                            if ((intPeso > 9))
                                intPeso = 2;
                        }

                        intResto = (intSoma % 11);
                        strDigito1 = ((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Substring((((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Length - 1));
                        strBase2 = (strBase.Substring(0, 10) + strDigito1);

                        if ((strBase2 == strOrigem))
                            retorno = true;

                        #endregion

                        break;
                    case "MS":

                        #region

                        strBase = (strOrigem.Trim() + "000000000").Substring(0, 9);

                        if ((strBase.Substring(0, 2) == "28"))
                        {
                            intSoma = 0;

                            for (intPos = 1; (intPos <= 8); intPos++)
                            {
                                intValor = int.Parse(strBase.Substring((intPos - 1), 1));
                                intValor = (intValor * (10 - intPos));
                                intSoma = (intSoma + intValor);
                            }

                            intResto = (intSoma % 11);
                            strDigito1 = ((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Substring((((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Length - 1));
                            strBase2 = (strBase.Substring(0, 8) + strDigito1);

                            if ((strBase2 == strOrigem))
                                retorno = true;
                        }

                        #endregion

                        break;

                    case "MG":

                        #region

                        strBase = (strOrigem.Trim() + "0000000000000").Substring(0, 13);
                        strBase2 = (strBase.Substring(0, 3) + ("0" + strBase.Substring(3, 8)));
                        intNumero = 2;

                        string strSoma = "";

                        for (intPos = 1; (intPos <= 12); intPos++)
                        {
                            intValor = int.Parse(strBase2.Substring((intPos - 1), 1));
                            intNumero = ((intNumero == 2) ? 1 : 2);
                            intValor = (intValor * intNumero);

                            intSoma = (intSoma + intValor);
                            strSoma += intValor.ToString();
                        }

                        intSoma = 0;

                        //Soma -se os algarismos, não o produto
                        for (int i = 0; i < strSoma.Length; i++)
                        {
                            intSoma += int.Parse(strSoma.Substring(i, 1));
                        }

                        intValor = int.Parse(strBase.Substring(8, 2));

                        var posicao = 8;
                        while (intValor < 10 && posicao > 1)
                        {
                            intValor = int.Parse(strBase.Substring((posicao - 1), 2));
                            posicao--;
                        }

                        strDigito1 = (intValor - intSoma).ToString();
                        strDigito1 = strDigito1.Substring(strDigito1.Length - 1, 1);
                        strBase2 = (strBase.Substring(0, 11) + strDigito1);

                        intSoma = 0;
                        intPeso = 3;

                        for (intPos = 1; (intPos <= 12); intPos++)
                        {
                            intValor = int.Parse(strBase.Substring((intPos - 1), 1));

                            if (intPeso < 2)
                                intPeso = 11;

                            intSoma += (intValor * intPeso);
                            intPeso--;
                        }

                        intResto = (intSoma % 11);
                        intValor = 11 - intResto;
                        strDigito2 = ((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Substring((((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Length - 1));

                        strBase2 = (strBase.Substring(0, 12) + strDigito2);

                        if (strBase2 == strOrigem)
                            retorno = true;


                        #endregion

                        break;

                    case "PA":

                        #region

                        strBase = (strOrigem.Trim() + "000000000").Substring(0, 9);

                        if ((strBase.Substring(0, 2) == "15"))
                        {
                            intSoma = 0;

                            for (intPos = 1; (intPos <= 8); intPos++)
                            {
                                intValor = int.Parse(strBase.Substring((intPos - 1), 1));
                                intValor = (intValor * (10 - intPos));
                                intSoma = (intSoma + intValor);
                            }

                            intResto = (intSoma % 11);
                            strDigito1 = ((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Substring((((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Length - 1));
                            strBase2 = (strBase.Substring(0, 8) + strDigito1);

                            if ((strBase2 == strOrigem))
                                retorno = true;
                        }

                        #endregion

                        break;

                    case "PB":

                        #region

                        strBase = (strOrigem.Trim() + "000000000").Substring(0, 9);
                        intSoma = 0;

                        for (intPos = 1; (intPos <= 8); intPos++)
                        {
                            intValor = int.Parse(strBase.Substring((intPos - 1), 1));
                            intValor = (intValor * (10 - intPos));
                            intSoma = (intSoma + intValor);
                        }

                        intResto = (intSoma % 11);
                        intValor = (11 - intResto);

                        if ((intValor > 9))
                            intValor = 0;

                        strDigito1 = Convert.ToString(intValor).Substring((Convert.ToString(intValor).Length - 1));
                        strBase2 = (strBase.Substring(0, 8) + strDigito1);

                        if ((strBase2 == strOrigem))
                            retorno = true;

                        #endregion

                        break;

                    case "PE":

                        #region

                        strBase = (strOrigem.Trim() + "00000000000000").Substring(0, 14);
                        intSoma = 0;
                        intPeso = 2;

                        for (intPos = 7; (intPos >= 1); intPos = (intPos + -1))
                        {
                            intValor = int.Parse(strBase.Substring((intPos - 1), 1));
                            intValor = (intValor * intPeso);
                            intSoma = (intSoma + intValor);
                            intPeso = (intPeso + 1);

                            if ((intPeso > 9))
                                intPeso = 2;
                        }

                        intResto = (intSoma % 11);
                        intValor = (11 - intResto);

                        if ((intValor > 9))
                            intValor = (intValor - 10);

                        strDigito1 = Convert.ToString(intValor).Substring((Convert.ToString(intValor).Length - 1));
                        strBase2 = (strBase.Substring(0, 7) + strDigito1);

                        if ((strBase2 == strOrigem.Substring(0, 8)))
                            retorno = true;

                        if (retorno)
                        {
                            intSoma = 0;
                            intPeso = 2;

                            for (intPos = 8; (intPos >= 1); intPos = (intPos + -1))
                            {
                                intValor = int.Parse(strBase.Substring((intPos - 1), 1));
                                intValor = (intValor * intPeso);
                                intSoma = (intSoma + intValor);
                                intPeso = (intPeso + 1);

                                if ((intPeso > 9))
                                    intPeso = 2;
                            }

                            intResto = (intSoma % 11);
                            intValor = (11 - intResto);

                            if ((intValor > 9))
                                intValor = (intValor - 10);

                            strDigito2 = Convert.ToString(intValor).Substring((Convert.ToString(intValor).Length - 1));
                            strBase2 = (strBase.Substring(0, 8) + strDigito2);

                            if ((strBase2 == strOrigem))
                                retorno = true;
                        }

                        #endregion

                        break;

                    case "PI":

                        #region

                        strBase = (strOrigem.Trim() + "000000000").Substring(0, 9);
                        intSoma = 0;

                        for (intPos = 1; (intPos <= 8); intPos++)
                        {
                            intValor = int.Parse(strBase.Substring((intPos - 1), 1));
                            intValor = (intValor * (10 - intPos));
                            intSoma = (intSoma + intValor);
                        }

                        intResto = (intSoma % 11);
                        strDigito1 = ((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Substring((((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Length - 1));
                        strBase2 = (strBase.Substring(0, 8) + strDigito1);

                        if ((strBase2 == strOrigem))
                            retorno = true;

                        #endregion

                        break;

                    case "PR":

                        #region

                        strBase = (strOrigem.Trim() + "0000000000").Substring(0, 10);
                        intSoma = 0;
                        intPeso = 2;

                        for (intPos = 8; (intPos >= 1); intPos = (intPos + -1))
                        {
                            intValor = int.Parse(strBase.Substring((intPos - 1), 1));
                            intValor = (intValor * intPeso);
                            intSoma = (intSoma + intValor);
                            intPeso = (intPeso + 1);

                            if ((intPeso > 7))
                                intPeso = 2;
                        }

                        intResto = (intSoma % 11);
                        strDigito1 = ((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Substring((((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Length - 1));
                        strBase2 = (strBase.Substring(0, 8) + strDigito1);
                        intSoma = 0;
                        intPeso = 2;

                        for (intPos = 9; (intPos >= 1); intPos = (intPos + -1))
                        {
                            intValor = int.Parse(strBase2.Substring((intPos - 1), 1));
                            intValor = (intValor * intPeso);
                            intSoma = (intSoma + intValor);
                            intPeso = (intPeso + 1);

                            if ((intPeso > 7))
                                intPeso = 2;
                        }

                        intResto = (intSoma % 11);
                        strDigito2 = ((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Substring((((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Length - 1));
                        strBase2 = (strBase2 + strDigito2);

                        if ((strBase2 == strOrigem))
                            retorno = true;

                        #endregion

                        break;

                    case "RJ":

                        #region

                        strBase = ($"{strOrigem.Trim()}00000000").Substring(0, 8);
                        intSoma = 0;
                        intPeso = 2;

                        for (int i = 0; i < 7; i++)
                        {
                            intValor = strBase.Substring(i, 1).ToInt();
                            intPeso = 8 - i;
                            if (i == 0)
                            {
                                intValor = (intValor * 2);
                                intSoma = (intSoma + intValor);
                            }
                            else
                            {
                                intValor = (intValor * intPeso);
                                intSoma = (intSoma + intValor);
                            }
                        }

                        intResto = (intSoma % 11);
                        strDigito1 = ((intResto <= 1) ? "0" : (11 - intResto).ToString());
                        strBase2 = ($"{strBase.Substring(0, 7)}{strDigito1}");

                        if ((strBase2 == strOrigem))
                            retorno = true;

                        #endregion

                        break;

                    case "RN": //Verficar com 10 digitos

                        #region

                        if (strOrigem.Length == 9)
                            strBase = (strOrigem.Trim() + "000000000").Substring(0, 9);
                        else if (strOrigem.Length == 10)
                            strBase = (strOrigem.Trim() + "000000000").Substring(0, 10);

                        if ((strBase.Substring(0, 2) == "20") && strBase.Length == 9)
                        {
                            intSoma = 0;

                            for (intPos = 1; (intPos <= 8); intPos++)
                            {
                                intValor = int.Parse(strBase.Substring((intPos - 1), 1));
                                intValor = (intValor * (10 - intPos));
                                intSoma = (intSoma + intValor);
                            }

                            intSoma = (intSoma * 10);
                            intResto = (intSoma % 11);
                            strDigito1 = ((intResto > 9) ? "0" : Convert.ToString(intResto)).Substring((((intResto > 9) ? "0" : Convert.ToString(intResto)).Length - 1));
                            strBase2 = (strBase.Substring(0, 8) + strDigito1);

                            if ((strBase2 == strOrigem))
                                retorno = true;
                        }
                        else if (strBase.Length == 10)
                        {
                            intSoma = 0;

                            for (intPos = 1; (intPos <= 9); intPos++)
                            {
                                intValor = int.Parse(strBase.Substring((intPos - 1), 1));
                                intValor = (intValor * (11 - intPos));
                                intSoma = (intSoma + intValor);
                            }

                            intSoma = (intSoma * 10);
                            intResto = (intSoma % 11);
                            strDigito1 = ((intResto > 10) ? "0" : Convert.ToString(intResto)).Substring((((intResto > 10) ? "0" : Convert.ToString(intResto)).Length - 1));
                            strBase2 = (strBase.Substring(0, 9) + strDigito1);

                            if ((strBase2 == strOrigem))
                                retorno = true;
                        }

                        #endregion

                        break;

                    case "RO":

                        #region

                        strBase = (strOrigem.Trim() + "000000000").Substring(0, 9);
                        strBase2 = strBase.Substring(3, 5);
                        intSoma = 0;

                        for (intPos = 1; (intPos <= 5); intPos++)
                        {
                            intValor = int.Parse(strBase2.Substring((intPos - 1), 1));
                            intValor = (intValor * (7 - intPos));
                            intSoma = (intSoma + intValor);
                        }

                        intResto = (intSoma % 11);
                        intValor = (11 - intResto);

                        if ((intValor > 9))
                            intValor = (intValor - 10);

                        strDigito1 = Convert.ToString(intValor).Substring((Convert.ToString(intValor).Length - 1));
                        strBase2 = (strBase.Substring(0, 8) + strDigito1);

                        if ((strBase2 == strOrigem))
                            retorno = true;
                        else
                        {
                            strBase = inscricaoEstadual.Trim().PadLeft(14, '0').Substring(0, 13);
                            intSoma = 0;

                            var peso = 6;
                            for (var i = 1; i <= 13; i++)
                            {
                                intValor = strBase.Substring((i - 1), 1).ToInt() * peso;
                                intSoma = (intSoma + intValor);
                                if (peso == 2)
                                    peso = 9;
                                else
                                    peso -= 1;
                            }

                            intResto = (intSoma % 11);
                            intValor = (11 - intResto);

                            if ((intValor > 9))
                                intValor = (intValor - 10);

                            var digitoCalculado = Convert.ToString(intValor).Substring((Convert.ToString(intValor).Length - 1)).ToInt();
                            var digito = inscricaoEstadual.Trim().PadLeft(14, '0').Substring(13, 1).ToInt();

                            if ((digitoCalculado == digito))
                                retorno = true;
                        }

                        #endregion

                        break;

                    case "RR":

                        #region

                        strBase = (strOrigem.Trim() + "000000000").Substring(0, 9);

                        if ((strBase.Substring(0, 2) == "24"))
                        {
                            intSoma = 0;

                            for (intPos = 1; (intPos <= 8); intPos++)
                            {
                                intValor = int.Parse(strBase.Substring((intPos - 1), 1));
                                intValor = intValor * intPos;
                                intSoma += intValor;
                            }

                            intResto = (intSoma % 9);
                            strDigito1 = Convert.ToString(intResto).Substring((Convert.ToString(intResto).Length - 1));
                            strBase2 = (strBase.Substring(0, 8) + strDigito1);

                            if ((strBase2 == strOrigem))
                                retorno = true;
                        }

                        #endregion

                        break;

                    case "RS":

                        #region

                        strBase = ($"{strOrigem.Trim()}0000000000").Substring(0, 10);

                        intSoma = 0;
                        intPeso = 2;

                        for (intPos = 9; (intPos >= 1); intPos = (intPos + -1))
                        {
                            intValor = strBase.Substring((intPos - 1), 1).ToInt();
                            intValor = (intValor * intPeso);
                            intSoma = (intSoma + intValor);
                            intPeso = (intPeso + 1);

                            if ((intPeso > 9))
                                intPeso = 2;
                        }

                        intResto = (intSoma % 11);
                        intValor = (11 - intResto);

                        if ((intValor > 9))
                            intValor = 0;

                        strBase2 = $"{strBase.Substring(0, 9)}{intValor.ToString().Substring((intValor.ToString().Length - 1))}";

                        if ((strBase2.Equals(strOrigem)))
                            retorno = true;

                        #endregion

                        break;

                    case "SC":

                        #region

                        strBase = ($"{strOrigem.Trim()}000000000").Substring(0, 9);
                        intSoma = 0;

                        for (int i = 0; i < 8; i++)
                        {
                            intValor = strBase.Substring(i, 1).ToInt();
                            intPeso = 9 - i;
                            intValor = (intValor * intPeso);
                            intSoma = (intSoma + intValor);
                        }

                        intResto = (intSoma % 11);
                        strDigito1 = (intResto <= 1) ? "0" : (11 - intResto).ToString();
                        strBase2 = $"{strBase.Substring(0, 8)}{strDigito1}";

                        if ((strBase2 == strOrigem))
                            retorno = true;

                        #endregion

                        break;

                    case "SE":

                        #region

                        strBase = (strOrigem.Trim() + "000000000").Substring(0, 9);
                        intSoma = 0;

                        for (intPos = 1; (intPos <= 8); intPos++)
                        {
                            intValor = int.Parse(strBase.Substring((intPos - 1), 1));
                            intValor = (intValor * (10 - intPos));
                            intSoma = (intSoma + intValor);
                        }

                        intResto = (intSoma % 11);
                        intValor = (11 - intResto);

                        if ((intValor > 9))
                            intValor = 0;

                        strDigito1 = Convert.ToString(intValor).Substring((Convert.ToString(intValor).Length - 1));
                        strBase2 = (strBase.Substring(0, 8) + strDigito1);

                        if ((strBase2 == strOrigem))
                            retorno = true;

                        #endregion

                        break;

                    case "SP":

                        #region

                        if ((strOrigem.Substring(0, 1) == "P"))
                        {
                            strBase = (strOrigem.Trim() + "0000000000000").Substring(0, 13);
                            strBase2 = strBase.Substring(1, 8);
                            intSoma = 0;
                            intPeso = 1;

                            for (intPos = 1; (intPos <= 8); intPos++)
                            {
                                intValor = int.Parse(strBase.Substring((intPos), 1));
                                intValor = (intValor * intPeso);
                                intSoma = (intSoma + intValor);
                                intPeso = (intPeso + 1);

                                if ((intPeso == 2))
                                    intPeso = 3;

                                if ((intPeso == 9))
                                    intPeso = 10;
                            }

                            intResto = (intSoma % 11);
                            strDigito1 = Convert.ToString(intResto).Substring((Convert.ToString(intResto).Length - 1));
                            strBase2 = (strBase.Substring(0, 9) + (strDigito1 + strBase.Substring(10, 3)));
                        }
                        else
                        {
                            strBase = (strOrigem.Trim() + "000000000000").Substring(0, 12);
                            intSoma = 0;
                            intPeso = 1;

                            for (intPos = 1; (intPos <= 8); intPos++)
                            {
                                intValor = int.Parse(strBase.Substring((intPos - 1), 1));
                                intValor = (intValor * intPeso);
                                intSoma = (intSoma + intValor);
                                intPeso = (intPeso + 1);

                                if ((intPeso == 2))
                                    intPeso = 3;

                                if ((intPeso == 9))
                                    intPeso = 10;
                            }

                            intResto = (intSoma % 11);
                            strDigito1 = Convert.ToString(intResto).Substring((Convert.ToString(intResto).Length - 1));
                            strBase2 = (strBase.Substring(0, 8) + (strDigito1 + strBase.Substring(9, 2)));
                            intSoma = 0;
                            intPeso = 2;

                            for (intPos = 11; (intPos >= 1); intPos = (intPos + -1))
                            {
                                intValor = int.Parse(strBase.Substring((intPos - 1), 1));
                                intValor = (intValor * intPeso);
                                intSoma = (intSoma + intValor);
                                intPeso = (intPeso + 1);

                                if ((intPeso > 10))
                                    intPeso = 2;
                            }

                            intResto = (intSoma % 11);
                            strDigito2 = Convert.ToString(intResto).Substring((Convert.ToString(intResto).Length - 1));
                            strBase2 = (strBase2 + strDigito2);
                        }

                        if ((strBase2 == strOrigem))
                            retorno = true;

                        #endregion

                        break;

                    case "TO":

                        #region

                        strBase = (strOrigem.Trim() + "00000000000").Substring(0, 11);

                        if ((("01,02,03,99".IndexOf(strBase.Substring(2, 2), 0, System.StringComparison.OrdinalIgnoreCase) + 1) > 0))
                        {
                            strBase2 = (strBase.Substring(0, 2) + strBase.Substring(4, 6));
                            intSoma = 0;

                            for (intPos = 1; (intPos <= 8); intPos++)
                            {
                                intValor = int.Parse(strBase2.Substring((intPos - 1), 1));
                                intValor = (intValor * (10 - intPos));
                                intSoma = (intSoma + intValor);
                            }

                            intResto = (intSoma % 11);
                            strDigito1 = ((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Substring((((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Length - 1));
                            strBase2 = (strBase.Substring(0, 10) + strDigito1);

                            if ((strBase2 == strOrigem))
                                retorno = true;
                        }

                        #endregion

                        break;

                }

                return retorno;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        ///  Adiciona zeros a esquerda na IE caso necessário
        /// </summary>
        /// <param name="uf"> Sigla da unidade federativa. </param>
        /// <param name="inscricaoEstadual"> Inscrição Estadual. </param>
        /// <returns></returns>
        public static string RetornarInscricaoEstadualFormatada(string uf, string inscricaoEstadual)
        {
            if (!ValidarInscricaoEstadual(uf, inscricaoEstadual))
            {
                var tentativa = 0;
                var ie = inscricaoEstadual;
                while (tentativa < 5)
                {
                    ie = $"0{ie}";
                    if (ValidarInscricaoEstadual(uf, ie))
                    {
                        return ie;
                    }
                    tentativa++;
                }

                return string.Empty;
            }
            else
            {
                return inscricaoEstadual;
            }
        }

        /// <summary>
        ///  Verifica se a <see cref="string"/> representa um e-mail válido.
        /// </summary>
        /// <param name="email"> E-mail. </param>
        /// <returns> <see cref="bool"/> true caso a string represente um e-mail válido, false caso contrário. </returns>
        public static bool EmailValido(this string email)
        {
            var result = true;

            if (!string.IsNullOrWhiteSpace(email))
            {
                result = Regex.IsMatch(email.Trim(), @"^((\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)\s*[;]{0,1}\s*)+$");

                // Verifica se o e-mail contem acentos ou ç.
                if (result && (!email.Equals(RemoverAcentos(email))))
                {
                    result = false;
                }
            }

            return result;
        }

        /// <summary>
        ///  Transforma os acentos em letras normais por exemplo É = E, Ç = C.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string RemoverAcentos(string text)
        {
            var retorno = new StringBuilder();
            var textos = text.Normalize(NormalizationForm.FormD).ToCharArray();

            foreach (char letter in textos)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                    retorno.Append(letter);
            }

            return retorno.Replace("&", "E").Replace("–", "-").ToString();
        }

        /// <summary>
        ///  Transforma os acentos em letras normais por exemplo É = E, Ç = C.
        ///  Este método deixa alguns caracteres presentes como . , $ -.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string RemoverAcentosECaracteresEspeciais(this string text)
        {
            if (text.IsNullOrWhiteSpace()) return text;

            var retorno = new StringBuilder();
            var textos = text.Normalize(NormalizationForm.FormD).ToCharArray();

            foreach (char letter in textos)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                    retorno.Append(letter);
            }

            return Regex.Replace(retorno.Replace("&", "E").Replace("–", "-").ToString(), @"[^a-zA-Z0-9,.$\s]", "");
        }

        /// <summary>
        ///  Substitui os caracteres de escape do XML pelos seus equivalentes.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string TratarParaXml(this string text)
        {
            if (text.IsNullOrWhiteSpace()) return text;

            return text.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace(@"'", "&apos;").Replace(@"""", "&quot;").Replace("–", "-");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string RemoverAcentosECaracteresEspeciaisPontosETracos(this string text)
        {
            if (text.IsNullOrWhiteSpace()) return text;

            var retorno = new StringBuilder();
            var textos = text.Normalize(NormalizationForm.FormD).ToCharArray();

            foreach (char letter in textos)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                    retorno.Append(letter);
            }

            return Regex.Replace(retorno.Replace("&", "E").Replace("–", "-").ToString(), @"[^a-zA-Z0-9\s]", "");
        }

        /// <summary>
        ///  Indicates whether a specified string is null, empty, or consists only of white-space characters.
        /// </summary>
        /// <param name="value"> The string to test. </param>
        /// <returns> true if the value parameter is null or System.String.Empty, or if value consists exclusively of white-space characters. </returns>
        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        ///  Formata o CNPJ, colocando pontuação e barra.
        /// </summary>
        /// <param name="cpfCnpj"></param>
        /// <returns></returns>
        public static string FormatarCnpjCpf(this string cpfCnpj)
        {
            var result = string.Empty;

            if (cpfCnpj.RemoverCaracteresNaoNumericos().Length == 14)
                result = cpfCnpj.RemoverCaracteresNaoNumericos().Insert(2, ".").Insert(6, ".").Insert(10, "/").Insert(15, "-");
            else if (cpfCnpj.RemoverCaracteresNaoNumericos().Length == 11)
                result = cpfCnpj.RemoverCaracteresNaoNumericos().Insert(3, ".").Insert(7, ".").Insert(11, "-");

            return result;
        }

        /// <summary>
        ///  Verifica se dois CNPJs pertencem a mesma empresa (Filial e Matriz ou Duas Filias).
        /// </summary>
        /// <param name="cnpjA"></param>
        /// <param name="cnpjB"></param>
        /// <returns></returns>
        public static bool CnpjFilialMatriz(string cnpjA, string cnpjB)
        {
            if (cnpjA.IsNullOrWhiteSpace() || cnpjB.IsNullOrWhiteSpace())
                return false;

            if (cnpjA.RemoverCaracteresNaoNumericos().Length != 14 || cnpjB.RemoverCaracteresNaoNumericos().Length != 14)
                return false;

            return cnpjA.RemoverCaracteresNaoNumericos().Substring(0, 8).Equals(cnpjB.RemoverCaracteresNaoNumericos().Substring(0, 8));
        }


        /// <summary>
        /// Obtém a data de vencimento em formato Juliano
        /// </summary>
        /// <param name="data">Data de vencimento</param>
        /// <returns>Data em formato Juliano</returns>
        public static string FormatarDataJuliano(this DateTime data)
        {
            return $"{data.DayOfYear.ToString("000")}{data.Year.ToString()[3]}";
        }

        /// <summary>
        ///  Calcula o hash MD5 de um byte[].
        /// </summary>
        /// <param name="dados"></param>
        /// <returns> Hash MD5. </returns>
        public static string GetMd5Hash(this byte[] dados)
        {
            using (var md5Hash = MD5.Create())
            {
                return Convert.ToBase64String(md5Hash.ComputeHash(dados));
            }
        }

        /// <summary>
        ///  Verifica se uma URL está com conectividade
        /// </summary>
        /// <param name="url"> Endereço. </param>
        /// <param name="timeout"> timeout em milisegundos. </param>
        /// <returns></returns>
        public static bool UrlComConectividade(string url, int timeout = 5000)
        {
            var request = WebRequest.Create(url) as HttpWebRequest;

            if (request != null)
            {
                request.Timeout = timeout;
                try
                {
                    using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                    {
                        return response?.StatusCode == HttpStatusCode.OK;
                    }
                }
                catch (WebException)
                {
                    return false;
                }
            }

            return false;
        }

        /// <summary>
        ///  Retorna o valor alfabético dos caracteres da string somados.
        ///  EX: A = 1, B = 2, AB = 3, CB = 5...
        /// </summary>
        /// <param name="s"> <see cref="string"/> </param>
        /// <returns> Valor dos caracteres da string somados. </returns>
        public static long ValorAlfabetico(this string s)
        {
            var result = 0L;

            foreach (var c in s.Trim().ToUpper())
            {
                result += (c - 64);
            }

            return result;
        }

        /// <summary>
        ///  Retorna uma string representadno um guid único sem os traços. Ex: 9F65CAC025D608D4603EFC9FD625D408
        /// </summary>
        /// <returns></returns>
        public static string GuidUnico()
        {
            return GerarGuidUnico().ToString().RemoverCaracteresEspeciais().ToUpper();
        }

        /// <summary>
        ///  Retorna uma string representadno um guid único sem os traços. Ex: 9F65CAC025D608D4603EFC9FD625D408
        /// </summary>
        /// <returns></returns>
        public static Guid GerarGuidUnico()
        {
            var bytes = new List<byte>();

            // Executa o processo duas vezes pois cada datetime gera um byte[8] e o guid necessita de um byte[16].
            bytes.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            Thread.Sleep(new Random().Next(0, 3000));
            bytes.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            return new Guid(bytes.ToArray());
        }

        /// <summary>
        ///  Determina o encode de um arquivo texto
        /// </summary>
        /// <param name="caminhoArquivoTexto"></param>
        /// <returns></returns>
        public static Encoding Encoding(string caminhoArquivoTexto)
        {
            using (var reader = new StreamReader(caminhoArquivoTexto, true))
            {
                reader.Peek(); // you need this!
                return reader.CurrentEncoding;
            }
        }

        /// <summary>
        ///  Informa ao Windows que o mesmo está em uso, para evitar que este entre em modo de hibernação.
        ///  Exemplo retirado de http://stackoverflow.com/questions/241222/need-to-disable-the-screen-saver-screen-locking-in-windows-c-net
        ///  Documentação em https://msdn.microsoft.com/pt-br/library/windows/desktop/aa373208(v=vs.85).aspx
        /// </summary>
        public static void PrevenirHibernacaoWindows()
        {
            SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS | EXECUTION_STATE.ES_DISPLAY_REQUIRED | EXECUTION_STATE.ES_SYSTEM_REQUIRED);
        }

        [FlagsAttribute]
        private enum EXECUTION_STATE : uint
        {
            ES_SYSTEM_REQUIRED = 0x00000001,
            ES_DISPLAY_REQUIRED = 0x00000002,
            ES_AWAYMODE_REQUIRED = 0x00000040,
            ES_CONTINUOUS = 0x80000000,
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);

    }
}