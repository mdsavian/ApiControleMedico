using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ApiControleMedico.Modelos;
using ApiControleMedico.Repositorio;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace ApiControleMedico.Services
{
    public class ClinicaService
    {
        protected readonly DbContexto<Clinica> ContextoClinicas;
        protected readonly EntidadeNegocio<Clinica> ClinicaNegocio = new EntidadeNegocio<Clinica>();

        public ClinicaService()
        {
            ContextoClinicas = new DbContexto<Clinica>("clinica");
        }

        public IEnumerable<Clinica> GetAll()
        {
            var clinicas =  ClinicaNegocio.GetAll(ContextoClinicas.Collection);
            return clinicas;
        }

        public Clinica GetOne(string id)
        {
            return ClinicaNegocio.GetOne(ContextoClinicas.Collection, id);
        }

        public Clinica SaveOne(Clinica context)
        {
            ClinicaNegocio.SaveOne(ContextoClinicas.Collection, context);

            return context;
        }

        public bool RemoveOne(string id)
        {
            return ClinicaNegocio.RemoveOne(ContextoClinicas.Collection, id);
        }

        public void SalvarLogo(string clinicaId, string nomeArquivo, string caminhoArquivo)
        {
            var gridFs = new GridFSBucket(ContextoClinicas.Database);
            string idLogo = "";

            var clinica = ContextoClinicas.Collection.Find(c => c.Id == clinicaId).FirstOrDefault();
            if (clinica != null)
            {
                if (!string.IsNullOrEmpty(clinica.LogoId))
                    gridFs.DeleteAsync(new ObjectId(clinica.LogoId));

                using (var foto = File.OpenRead(caminhoArquivo))
                {
                    var task = Task.Run(() =>
                    {
                        return gridFs.UploadFromStreamAsync(nomeArquivo, foto);
                    });
                    idLogo = task.Result.ToString();
                }

                clinica.LogoId = idLogo;
                ClinicaNegocio.SaveOne(ContextoClinicas.Collection, clinica);

            }
        }

        public byte[] DownloadLogo(string clinicaId)
        {
            var gridFs = new GridFSBucket(ContextoClinicas.Database);
            try
            {
                var task = gridFs.DownloadAsBytesByNameAsync($"clinica-{clinicaId}.jpeg");
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