using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ApiControleMedico.Modelos;
using ApiControleMedico.Repositorio;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace ApiControleMedico.Services
{
    public class ProntuarioService
    {
        protected readonly DbContexto<Prontuario> ContextoProntuarios;
        protected readonly EntidadeNegocio<Prontuario> ProntuarioNegocio = new EntidadeNegocio<Prontuario>();

        private static IHostingEnvironment _hostingEnvironment;

        public ProntuarioService(IHostingEnvironment hostingEnvironment)
        {
            ContextoProntuarios = new DbContexto<Prontuario>("prontuario");
            _hostingEnvironment = hostingEnvironment;

        }

        public IEnumerable<Prontuario> GetAll()
        {
            var prontuarios = ProntuarioNegocio.GetAll(ContextoProntuarios.Collection);
            return prontuarios;
        }

        public Prontuario GetOne(string id)
        {
            return ProntuarioNegocio.GetOne(ContextoProntuarios.Collection, id);
        }

        public Prontuario SaveOne(Prontuario prontuario)
        {
            ProntuarioNegocio.SaveOne(ContextoProntuarios.Collection, prontuario);

            return prontuario;
        }

        public bool RemoveOne(string id)
        {
            return ProntuarioNegocio.RemoveOne(ContextoProntuarios.Collection, id);
        }

        public void SaveMany(Collection<Prontuario> prontuarios)
        {
            foreach (var prontuario in prontuarios)
            {
                if (ContextoProntuarios.Collection.Find(c => c.Descricao == prontuario.Descricao)
                        .FirstOrDefault() != null)
                    continue;

                ProntuarioNegocio.SaveOne(ContextoProntuarios.Collection, prontuario);
            }
        }

        internal Prontuario SalvarArquivos(IFormFile file)
        {
            string folderName = "Upload";
            string webRootPath = _hostingEnvironment.ContentRootPath;
            string newPath = Path.Combine(webRootPath, folderName);

            if (!Directory.Exists(newPath))
                Directory.CreateDirectory(newPath);
            if (file.Length > 0 && !string.IsNullOrEmpty(file.Name))
            {
                string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

                string fullPath = Path.Combine(newPath, fileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                    file.CopyTo(stream);

                var pacienteId = file.Name.Split("=")[1];
                var paciente = new PacienteService().GetOne(pacienteId);

                if (paciente != null)
                {
                    var gridFs = new GridFSBucket(ContextoProntuarios.Database);

                    var prontuario = ContextoProntuarios.Collection.Find(c => c.PacienteId == pacienteId).FirstOrDefault();

                    if (prontuario == null)
                        prontuario = new Prontuario { PacienteId = pacienteId, Anexos = new List<AnexoProntuario>() };

                    using (var arquivo = File.OpenRead(fullPath))
                    {
                        var task = Task.Run(() =>
                        {
                            return gridFs.UploadFromStreamAsync(fileName, arquivo);
                        });

                        string idArquivo = task.Result.ToString();
                        var nomeSplit = fileName.Split(".");

                        if (prontuario.Anexos == null)
                            prontuario.Anexos = new List<AnexoProntuario>();

                        prontuario.Anexos.Add(new AnexoProntuario
                        {
                            Id = idArquivo,
                            NomeArquivo = nomeSplit[0],
                            ExtensaoArquivo = nomeSplit[1].ToUpper(),
                            ContentType = file.ContentType
                        });

                        this.SaveOne(prontuario);
                        return prontuario;
                    }
                }

                File.Delete(fullPath);
            }

            return null;
        }

        internal Prontuario DeletarArquivo(string prontuarioId, string arquivoId)
        {
            var prontuario = ContextoProntuarios.Collection.Find(c => c.Id == prontuarioId).FirstOrDefault();
            if (prontuario != null)
            {
                try
                {                   
                    var arquivo = prontuario.Anexos.FirstOrDefault(c => c.Id == arquivoId);
                    if (arquivo != null)
                    {
                        var gridFs = new GridFSBucket(ContextoProntuarios.Database);
                        var objectId = new ObjectId(arquivoId);
                        gridFs.Delete(objectId);

                        prontuario.Anexos.Remove(arquivo);

                        prontuario = this.SaveOne(prontuario);
                    }

                }
                catch (Exception ex)
                {                    
                }

                return prontuario;
            }

            return null;
        }

        internal byte[] DownloadArquivo(string idArquivo)
        {
            var gridFs = new GridFSBucket(ContextoProntuarios.Database);
            try
            {
                var objectId = new ObjectId(idArquivo);
                var bytes = gridFs.DownloadAsBytes(objectId);
                return bytes;
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        internal ActionResult<Prontuario> BuscarPorPaciente(string pacienteId)
        {
            var prontuario = this.ContextoProntuarios.Collection.Find(c => c.PacienteId == pacienteId).FirstOrDefault();

            if (prontuario == null)
            {
                prontuario = new Prontuario
                {
                    PacienteId = pacienteId,
                    Anexos = new List<AnexoProntuario>()
                };
                prontuario = this.SaveOne(prontuario);
            }

            return prontuario;
        }
    }
}