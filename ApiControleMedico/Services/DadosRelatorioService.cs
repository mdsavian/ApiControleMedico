using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiControleMedico.Modelos;
using ApiControleMedico.Modelos.NaoPersistidos;
using ApiControleMedico.Repositorio;
using ApiControleMedico.Uteis;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ApiControleMedico.Services
{
    public class DadosRelatorioService
    {
        protected readonly DbContexto<Paciente> ContextoPacientes;
        protected readonly EntidadeNegocio<Paciente> PacienteNegocio = new EntidadeNegocio<Paciente>();

        public DadosRelatorioService()
        {
            ContextoPacientes = new DbContexto<Paciente>("paciente");
        }

        public async void ValidarDados(List<DadosRelatorioUnimed> dados)
        {
            var builder = Builders<Paciente>.Filter;
            List<Paciente> novosContextoPacientes = new List<Paciente>();
            foreach (var dado in dados)
            {
                var filter = builder.Eq("NomeCompleto", dado.Beneficiario) & builder.Eq("NumeroCartao", dado.Carteira);

                var paciente = await ContextoPacientes.Collection.Find(filter).FirstOrDefaultAsync();

                if (paciente == null)
                {
                    novosContextoPacientes.Add(new Paciente
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                        NomeCompleto = dado.Beneficiario.ToUpper(),
                        NumeroCartao = dado.Carteira,
                        //Convenio = dado.Convenio,
                        TipoPlano = dado.TipoPlano
                    });


                }
            }
            if (novosContextoPacientes.HasItems())
                await ContextoPacientes.Collection.InsertManyAsync(novosContextoPacientes);
        }
    }
}
