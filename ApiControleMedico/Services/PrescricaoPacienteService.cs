using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ApiControleMedico.Modelos;
using ApiControleMedico.Repositorio;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace ApiControleMedico.Services
{
    public class PrescricaoPacienteService
    {
        protected readonly DbContexto<PrescricaoPaciente> ContextoPrescricaoPacientes;
        protected readonly EntidadeNegocio<PrescricaoPaciente> PrescricaoPacienteNegocio = new EntidadeNegocio<PrescricaoPaciente>();

        public PrescricaoPacienteService()
        {
            ContextoPrescricaoPacientes = new DbContexto<PrescricaoPaciente>("prescricaoPaciente");
        }

        public IEnumerable<PrescricaoPaciente> GetAll()
        {
            var prescricaoPacientes = PrescricaoPacienteNegocio.GetAll(ContextoPrescricaoPacientes.Collection);
            return prescricaoPacientes;
        }

        public PrescricaoPaciente BuscarPorId(string id)
        {
            return PrescricaoPacienteNegocio.GetOne(ContextoPrescricaoPacientes.Collection, id);
        }

        public PrescricaoPaciente SaveOne(PrescricaoPaciente context)
        {
            PrescricaoPacienteNegocio.SaveOne(ContextoPrescricaoPacientes.Collection, context);
            return context;
        }

        public bool RemoveOne(string id)
        {
            return PrescricaoPacienteNegocio.RemoveOne(ContextoPrescricaoPacientes.Collection, id);
        }

        public void SaveMany(Collection<PrescricaoPaciente> prescricaoPacientes)
        {
            foreach (var prescricaoPaciente in prescricaoPacientes)
            {
                if (ContextoPrescricaoPacientes.Collection.Find(c => c.Descricao == prescricaoPaciente.Descricao)
                        .FirstOrDefault() != null)
                    continue;

                PrescricaoPacienteNegocio.SaveOne(ContextoPrescricaoPacientes.Collection, prescricaoPaciente);
            }
        }

        internal ActionResult<List<PrescricaoPaciente>> BuscarPorPaciente(string pacienteId, string usuarioId, string clinicaId)
        {
            var medicosId = new MedicoService().BuscarMedicosPorUsuario(usuarioId, clinicaId, false).Select(c => c.Id);

            return ContextoPrescricaoPacientes.Collection.AsQueryable().Where(c => c.PacienteId == pacienteId && medicosId.Contains(c.MedicoId)).OrderByDescending(c => c.Data).ToList();
        }
    }
}