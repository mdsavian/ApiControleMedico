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
        protected readonly DbContexto<Paciente> ContextoPacientes;
        protected readonly EntidadeNegocio<Paciente> PacienteNegocio = new EntidadeNegocio<Paciente>();

        public PacienteService()
        {
            ContextoPacientes = new DbContexto<Paciente>("paciente");
        }

        public IEnumerable<Paciente> GetAll()
        {
            var pacientes = PacienteNegocio.GetAll(ContextoPacientes.Collection);
            return pacientes;
        }

        public Paciente GetOne(string id)
        {
            return PacienteNegocio.GetOne(ContextoPacientes.Collection, id);
        }

        public Paciente SaveOne(Paciente context)
        {
            PacienteNegocio.SaveOne(ContextoPacientes.Collection, context);

            return context;
        }

        public bool RemoveOne(string id)
        {
            return PacienteNegocio.RemoveOne(ContextoPacientes.Collection, id);
        }

        public ActionResult<List<Paciente>> TodosGestantesFiltrandoMedico(string medicoId)
        {
            // falar com samir para ver se o paciente vai ser relacionado com o médico

            return ContextoPacientes.Collection.Find(c => !string.IsNullOrEmpty(c.DiaGestacao) && !string.IsNullOrEmpty(c.SemanaGestacao)).ToList().OrderByDescending(c => c.SemanaGestacao).ToList();
        }
    }
}