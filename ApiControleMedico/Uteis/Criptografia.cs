using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiControleMedico.Modelos;
using ApiControleMedico.Services;
using CryptSharp;
using Microsoft.AspNetCore.Mvc;

namespace ApiControleMedico.Uteis
{
    public static class Criptografia
    {
        public static string Codifica(string senha)
        {
            return Crypter.MD5.Crypt(senha);
        }

        public static bool Compara(string senhaAComparar, string hash)
        {
            return Crypter.CheckPassword(senhaAComparar, hash);
        }
    }
}
