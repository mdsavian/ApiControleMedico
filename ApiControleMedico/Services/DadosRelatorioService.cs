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
        protected readonly DbContexto<Paciente> Pacientes;
        protected readonly EntidadeNegocio<Paciente> PacienteNegocio = new EntidadeNegocio<Paciente>();

        public DadosRelatorioService()
        {
            Pacientes = new DbContexto<Paciente>("paciente");
        }

        public async void ValidarDados(List<DadosRelatorioUnimed> dados)
        {
            var builder = Builders<Paciente>.Filter;
            List<Paciente> novosPacientes = new List<Paciente>();
            foreach (var dado in dados)
            {
                var filter = builder.Eq("NomeCompleto", dado.Beneficiario) & builder.Eq("NumeroCartao", dado.Carteira);

                var paciente = await Pacientes.Collection.Find(filter).FirstOrDefaultAsync();

                if (paciente == null)
                {
                    novosPacientes.Add(new Paciente
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                        NomeCompleto = dado.Beneficiario.ToUpper(),
                        NumeroCartao = dado.Carteira,
                        Convenio = dado.Convenio,
                        TipoPlano = dado.TipoPlano
                    });


                }
            }
            if (novosPacientes.HasItems())
                await Pacientes.Collection.InsertManyAsync(novosPacientes);
        }
    }
}
