﻿using System;
using System.Collections.Generic;
using System.Linq;
using ApiControleMedico.Modelos;
using ApiControleMedico.Repositorio;
using ApiControleMedico.Uteis;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace ApiControleMedico.Services
{
    public class ConfiguracaoAgendaService
    {
        protected readonly DbContexto<ConfiguracaoAgenda> ContextoConfiguracaoAgenda;
        protected readonly EntidadeNegocio<ConfiguracaoAgenda> ConfiguracaoAgendaNegocio = new EntidadeNegocio<ConfiguracaoAgenda>();

        public ConfiguracaoAgendaService()
        {
            ContextoConfiguracaoAgenda = new DbContexto<ConfiguracaoAgenda>("configuracaoAgenda");
        }

        public IEnumerable<ConfiguracaoAgenda> GetAll()
        {
            return ConfiguracaoAgendaNegocio.GetAll(ContextoConfiguracaoAgenda.Collection).ToList();
        }

        public ConfiguracaoAgenda GetOne(string id)
        {
            return ConfiguracaoAgendaNegocio.GetOne(ContextoConfiguracaoAgenda.Collection, id);
        }

        public ConfiguracaoAgenda SaveOne(ConfiguracaoAgenda configuracaoAgenda)
        {
            configuracaoAgenda = ConfigurarAgendaMedico(configuracaoAgenda);
            ConfiguracaoAgendaNegocio.SaveOne(ContextoConfiguracaoAgenda.Collection, configuracaoAgenda);
            return configuracaoAgenda;
        }


        public bool RemoveOne(string id)
        {
            return ConfiguracaoAgendaNegocio.RemoveOne(ContextoConfiguracaoAgenda.Collection, id);
        }

        public static ConfiguracaoAgenda ConfigurarAgendaMedico(ConfiguracaoAgenda medicoConfiguracaoAgenda)
        {

            medicoConfiguracaoAgenda.DiasNaoConfigurados = medicoConfiguracaoAgenda.ConfiguracaoAgendaDias.Where(c => !c.Configurado).Select(c => c.Dia).ToArray();
            var configurados = medicoConfiguracaoAgenda.ConfiguracaoAgendaDias.Where(c => c.Configurado).ToList();

            var minPrimeiro = configurados.Where(c => !c.PrimeiroHorarioInicial.IsNullOrWhiteSpace()).Min(c => c.PrimeiroHorarioInicial);
            var minSegundo = configurados.Where(c => !c.SegundoHorarioInicial.IsNullOrWhiteSpace()).Min(c => c.SegundoHorarioInicial);

            var maxPrimeiro = configurados.Where(c => !c.PrimeiroHorarioFinal.IsNullOrWhiteSpace()).Max(c => c.PrimeiroHorarioFinal);
            var maxSegundo = configurados.Where(c => !c.SegundoHorarioFinal.IsNullOrWhiteSpace()).Max(c => c.SegundoHorarioFinal);

            medicoConfiguracaoAgenda.PrimeiroHorario = minPrimeiro.ToInt() < minSegundo.ToInt() ? minPrimeiro.Substring(0, 2) : minSegundo.Substring(0, 2);
            medicoConfiguracaoAgenda.UltimoHorario = maxPrimeiro.ToInt() > maxSegundo.ToInt() ? maxPrimeiro.Substring(0, 2) : maxSegundo.Substring(0, 2);

            foreach (var dias in configurados.Where(c => !c.SegundoHorarioInicial.IsNullOrWhiteSpace()).ToList())
            {
                var intervaloInicial = new TimeSpan(dias.PrimeiroHorarioFinal.Substring(0, 2).ToInt(), dias.PrimeiroHorarioFinal.Substring(2, 2).ToInt() + 1, 0);
                var intervaloFinal = new TimeSpan(dias.SegundoHorarioInicial.Substring(0, 2).ToInt(), dias.SegundoHorarioInicial.Substring(2, 2).ToInt() - 1, 0);

                dias.HorarioInicioIntervalo = $"{intervaloInicial.Hours:00}{intervaloInicial.Minutes:00}";
                dias.HorarioFimIntervalo = $"{intervaloFinal.Hours:00}{intervaloFinal.Minutes:00}";
            }

            using (var contexto = new DbContexto<ConfiguracaoAgenda>("configuracaoAgenda"))
            {
                var configNegocio = new EntidadeNegocio<ConfiguracaoAgenda>();
                configNegocio.SaveOne(contexto.Collection, medicoConfiguracaoAgenda);
            }
            return medicoConfiguracaoAgenda;

        }

        internal ActionResult<ConfiguracaoAgenda> BuscarConfiguracaoAgenda(string medicoId, string clinicaId)
        {
            return ContextoConfiguracaoAgenda.Collection.Find(c => c.MedicoId == medicoId && c.ClinicaId == clinicaId).FirstOrDefault();
        }
    }
}