using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiControleMedico.Modelos;
using ApiControleMedico.Repositorio;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace ApiControleMedico.Services
{
    public class PacienteService
    {
        protected readonly DbContexto<Paciente> Pacientes;
        protected readonly EntidadeNegocio<Paciente> PacienteNegocio = new EntidadeNegocio<Paciente>();

        public PacienteService()
        {
            Pacientes = new DbContexto<Paciente>("paciente");
        }

        public async Task<IEnumerable<Paciente>> GetAllAsync()
        {
            var pacientes = await PacienteNegocio.GetAllAsync(Pacientes.Collection);
            return pacientes;
        }

        public Task<Paciente> GetOneAsync(string id)
        {
            return PacienteNegocio.GetOneAsync(Pacientes.Collection, id);
        }

        public async Task<Paciente> SaveOneAsync(Paciente context)
        {
            await PacienteNegocio.SaveOneAsync(Pacientes.Collection, context);

            return context;
        }

        public Task<bool> RemoveOneAsync(string id)
        {
            return PacienteNegocio.RemoveOneAsync(Pacientes.Collection, id);
        }

        public ActionResult<List<Paciente>> TodosGestantesFiltrandoMedico(string medicoId)
        {
            // falar com samir para ver se o paciente vai ser relacionado com o médico

            return Pacientes.Collection.Find(c => !string.IsNullOrEmpty(c.DiaGestacao) && !string.IsNullOrEmpty(c.SemanaGestacao)).ToList().OrderByDescending(c=> c.SemanaGestacao).ToList();
        }
    }
}