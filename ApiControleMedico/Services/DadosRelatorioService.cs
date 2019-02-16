using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiControleMedico.Modelos;
using ApiControleMedico.Modelos.NaoPersistidos;
using ApiControleMedico.Repositorio;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ApiControleMedico.Services
{
    public class DadosRelatorioService
    {
        protected readonly DbContexto<Paciente> Pacientes;
        protected readonly EntidadeNegocio<Paciente, Paciente> PacienteNegocio = new EntidadeNegocio<Paciente, Paciente>();

        public DadosRelatorioService()
        {
            Pacientes = new DbContexto<Paciente>("paciente");
        }

        public async void ValidarDados(List<DadosRelatorioUnimed> dados)
        {
            var builder = Builders<Paciente>.Filter;

            foreach (var dado in dados)
            {
                var filter = builder.Eq("nomecompleto", dado.Beneficiario) & builder.Eq("NumeroCartao", dado.Carteira);

                var paciente = await Pacientes.Collection.Find(filter).FirstOrDefaultAsync();

                if (paciente == null)
                {
                    paciente = new Paciente
                    {
                        NomeCompleto = dado.Beneficiario,
                        NumeroCartao = dado.Carteira,
                        Convenio = dado.Convenio,
                        TipoPlano = dado.TipoPlano
                    };

                    Pacientes.Collection.InsertOne(paciente);
                }
            }
        }
    }
}
