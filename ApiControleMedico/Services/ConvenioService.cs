﻿using System;
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
            var conveniosMedicosId = new MedicoService().GetOne(medicoId).ConveniosId;

            var conveniosMedicos = new ConvenioService().GetAll().AsQueryable().Where(c => conveniosMedicosId.Contains(c.Id)).ToList();         

            return conveniosMedicos;

        }
    }
}