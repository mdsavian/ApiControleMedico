using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiControleMedico.Modelos;
using ApiControleMedico.Repositorio;
using MongoDB.Driver;

namespace ApiControleMedico.Services
{
    public class ConvenioService
    {
        protected readonly DbContexto<Convenio> ContextoConvenio;
        protected readonly EntidadeNegocio<Convenio> ConvenioNegocio = new EntidadeNegocio<Convenio>();

        public ConvenioService()
        {
            ContextoConvenio = new DbContexto<Convenio>("convenio");
        }

        public  IEnumerable<Convenio> GetAll()
        {
            var convenios = ConvenioNegocio.GetAll(ContextoConvenio.Collection);
            return convenios;
        }

        public Convenio GetOne(string id)
        {
            return ConvenioNegocio.GetOne(ContextoConvenio.Collection, id);
        }

        public Convenio SaveOne(Convenio convenio)
        {
            ConvenioNegocio.SaveOne(ContextoConvenio.Collection, convenio);
            return convenio;
        }


        public bool RemoveOne(string id)
        {
            return ConvenioNegocio.RemoveOne(ContextoConvenio.Collection, id);
        }

        public List<Medico> BuscarMedicosPorConvenio(string convenioId)
        {
            List<Medico> medicos = new List<Medico>();
            try
            {
                medicos = new MedicoService().GetAll().ToList();
                medicos = medicos.Where(c => c.ConveniosId != null && c.ConveniosId.Contains(convenioId)).ToList();
            }
            catch (Exception ex)
            {

            }

            return medicos;
        }

        public List<Convenio> TodosFiltrandoMedico(string medicoId)
        {
            var conveniosMedicos = new MedicoService().GetOne(medicoId).Convenios;
            try
            {
                var filter = Builders<Convenio>.Filter.Nin(c => c.Id, conveniosMedicos.Select(c => c.Id));
                var convenios = ContextoConvenio.Collection.Find(filter).ToList();
                return convenios;
            }
            catch (Exception ex)
            {

            }

            return ContextoConvenio.Collection.Find(c => true).ToList();

        }
    }
}