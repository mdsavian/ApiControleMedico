using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ApiControleMedico.Modelos;
using ApiControleMedico.Repositorio;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace ApiControleMedico.Services
{
    public class PacienteService
    {
        protected readonly DbContexto<Paciente> contextoPacientes;
        protected readonly DbContexto<Agendamento> ContextoAgendamentos;

        protected readonly EntidadeNegocio<Paciente> PacienteNegocio = new EntidadeNegocio<Paciente>();

        public PacienteService()
        {
            contextoPacientes = new DbContexto<Paciente>("paciente");
            ContextoAgendamentos = new DbContexto<Agendamento>("agendamento");

        }

        public IEnumerable<Paciente> GetAll()
        {
            var pacientes = PacienteNegocio.GetAll(contextoPacientes.Collection).OrderBy(c=> c.NomeCompleto).ToList();
            return pacientes;
        }

        public Paciente GetOne(string id)
        {
            return PacienteNegocio.GetOne(contextoPacientes.Collection, id);
        }

        public Paciente SaveOne(Paciente paciente)
        {
            PacienteNegocio.SaveOne(contextoPacientes.Collection, paciente);

            return paciente;
        }

        public bool RemoveOne(string id)
        {
            return PacienteNegocio.RemoveOne(contextoPacientes.Collection, id);
        }

        public ActionResult<List<Paciente>> TodosGestantesFiltrandoMedico(string medicoId)
        {
            var pacientesId = ContextoAgendamentos.Collection.AsQueryable().Where(c => c.MedicoId == medicoId).Select(c => c.PacienteId).ToList();
            var pacientesGestantes = contextoPacientes.Collection.Find(c => !string.IsNullOrEmpty(c.DiaGestacao) && !string.IsNullOrEmpty(c.SemanaGestacao) && pacientesId.Contains(c.Id)).ToList().OrderByDescending(c => c.SemanaGestacao).ToList();
            
            return pacientesGestantes;
        }

        public string SalvarFoto(string pacienteId, string nomeArquivo, string caminhoArquivo)
        {
            var gridFs = new GridFSBucket(contextoPacientes.Database);
            string idFoto = "";

            var paciente = contextoPacientes.Collection.Find(c => c.Id == pacienteId).FirstOrDefault();
            if (paciente != null)
            {

                if (!string.IsNullOrEmpty(paciente.FotoId))
                    gridFs.DeleteAsync(new ObjectId(paciente.FotoId));

                using (var foto = File.OpenRead(caminhoArquivo))
                {
                    var task = Task.Run(() =>
                    {
                        return gridFs.UploadFromStreamAsync(nomeArquivo, foto);
                    });
                    idFoto = task.Result.ToString();
                }

                paciente.FotoId = idFoto;
                PacienteNegocio.SaveOne(contextoPacientes.Collection, paciente);
            }

            return idFoto;
        }

        public byte[] DownloadFoto(string pacienteId)
        {
            var gridFs = new GridFSBucket(contextoPacientes.Database);
            try
            {
                var task = gridFs.DownloadAsBytesByNameAsync($"paciente-{pacienteId}.jpeg");
                Task.WaitAll(task);
                var bytes = task.Result;
                return bytes;
            }
            catch (Exception ex)
            {

            }

            return null;
        }
    }
}