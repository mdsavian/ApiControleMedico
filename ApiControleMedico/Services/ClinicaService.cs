using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ApiControleMedico.Modelos;
using ApiControleMedico.Repositorio;
using MongoDB.Driver;

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

        public async Task<IEnumerable<Clinica>> GetAllAsync()
        {
            var clinicas = await ClinicaNegocio.GetAllAsync(ContextoClinicas.Collection);
            return clinicas;
        }

        public Task<Clinica> GetOneAsync(string id)
        {
            return ClinicaNegocio.GetOneAsync(ContextoClinicas.Collection, id);
        }

        public async Task<Clinica> SaveOneAsync(Clinica context)
        {
            await ClinicaNegocio.SaveOneAsync(ContextoClinicas.Collection, context);

            return context;
        }

        public Task<bool> RemoveOneAsync(string id)
        {
            return ClinicaNegocio.RemoveOneAsync(ContextoClinicas.Collection, id);
        }
    }
}