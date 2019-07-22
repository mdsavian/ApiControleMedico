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
    }
}