using System.Collections.Generic;
using System.Threading.Tasks;
using ApiControleMedico.Modelos;
using ApiControleMedico.Modelos.NaoPersistidos;
using ApiControleMedico.Repositorio;

namespace ApiControleMedico.Services
{
    public class EnderecoService
    {
        public Endereco BuscaEnderecoWebService(string cep)
        {
            
            var wsCorreioAtendeCliente = new WsCorreios.AtendeClienteClient();
            var retorno = wsCorreioAtendeCliente.consultaCEPAsync(cep).Result.@return;
            var endereco = new Endereco
            {
                Bairro = retorno.bairro,
                Cep = retorno.cep,
                Cidade = retorno.cidade,
                Rua = retorno.end,
                Complemento = retorno.complemento2,
                Uf = retorno.uf
            };

            return endereco;
        }
    }

}