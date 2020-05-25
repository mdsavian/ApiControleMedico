using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using ApiControleMedico.Modelos;
using ApiControleMedico.Modelos.Enums;
using ApiControleMedico.Services;
using ApiControleMedico.Uteis;
using MongoDB.Driver;

namespace ApiControleMedico.Repositorio
{
    public class DadosFixos
    {
        public void AlimentarTabelas()
        {

            AlimentaTabelaEspecialidade();
            AlimentaTabelaFormaPagamento();
            AlimentaAgendamentos();
        }

        private void AlimentaAgendamentos()
        {
            var agendamentoService = new AgendamentoService();
            var contextoAgendamentos = new DbContexto<Agendamento>("agendamento");

            foreach (var aa in contextoAgendamentos.Collection.AsQueryable().ToList())
            {
                agendamentoService.RemoveOne(aa.Id);
            }

            var medicoService = new MedicoService();
            var pacienteService = new PacienteService();
            var convenioService = new ConvenioService();
            var cirurgiaService = new CirurgiaService();
            var procedimentoService = new ProcedimentoService();

            var contextoMedico = new DbContexto<Medico>("medico");
            var contextoPaciente = new DbContexto<Paciente>("paciente");
            var contextoConvenio = new DbContexto<Convenio>("convenio");
            var contextoCirurgia = new DbContexto<Cirurgia>("cirurgia");
            var contextoClinica = new DbContexto<Clinica>("clinica");
            var contextoProcedimento = new DbContexto<Procedimento>("procedimento");

            var csvAgendamento = Resource.agendas_novembro_importacao;
            using (var reader = new StringReader(csvAgendamento))
            {
                string line;
                try
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        //Médico;Paciente;Data Agendamento;Tipo;Telefone;Descricao;Convênio;Primeiro;OBS;
                        var linhaAgendamento = line.Split(';');

                        if (!linhaAgendamento[0].Trim().IsNullOrWhiteSpace() && linhaAgendamento[2].ToDateTimeOrNull() != null)
                        {
                            var agendamento = new Agendamento();                            

                            var nomeMedico = linhaAgendamento[0].ToUpper();
                            var nomePaciente = linhaAgendamento[1].ToUpper();
                            var dataAgendamento = linhaAgendamento[2].ToDateTime();
                            var tipoAgendamento = linhaAgendamento[3];
                            var telefone = linhaAgendamento[4];
                            var descricao = linhaAgendamento[5].ToUpper();
                            var convenio = linhaAgendamento[6].ToUpper();
                            var primeiroAtendimento = linhaAgendamento[7];
                            var EnumTipoAgendamento = (ETipoAgendamento)Enum.Parse(typeof(ETipoAgendamento), tipoAgendamento);

                            agendamento.DataAgendamento = dataAgendamento.Date;
                            agendamento.Observacao = linhaAgendamento[8];
                            agendamento.HoraInicial = dataAgendamento.FormatarHora24().Replace(":", "");
                            agendamento.HoraFinal = dataAgendamento.AddMinutes(20).FormatarHora24().Replace(":", "");
                            agendamento.PrimeiraConsulta = primeiroAtendimento == "Não" ? false : true;

                            switch (EnumTipoAgendamento)
                            {
                                case ETipoAgendamento.Cirurgia:
                                    {
                                        var cirurgiaCadastrada = contextoCirurgia.Collection.Find(c => c.Descricao == descricao).FirstOrDefault();
                                        if (cirurgiaCadastrada == null)
                                        {
                                            cirurgiaCadastrada = new Cirurgia { Descricao = descricao };
                                            cirurgiaCadastrada = cirurgiaService.SaveOne(cirurgiaCadastrada);                                            
                                        }
                                        agendamento.CirurgiaId = cirurgiaCadastrada.Id;
                                        agendamento.TipoAgendamento = ETipoAgendamento.Cirurgia;
                                        break;
                                    }
                                case ETipoAgendamento.Procedimento:
                                    {
                                        var procedimentoCadastrada = contextoProcedimento.Collection.Find(c => c.Descricao == descricao).FirstOrDefault();
                                        if (procedimentoCadastrada == null)
                                        {
                                            procedimentoCadastrada = new Procedimento { Descricao = descricao };
                                            procedimentoCadastrada = procedimentoService.SaveOne(procedimentoCadastrada);                                            
                                        }
                                        agendamento.ProcedimentoId = procedimentoCadastrada.Id;
                                        agendamento.TipoAgendamento = ETipoAgendamento.Procedimento;
                                        break;
                                    }
                                default:
                                    {
                                        agendamento.TipoAgendamento = EnumTipoAgendamento;
                                        break;
                                    }
                            }

                            var convenioCadastrado = contextoConvenio.Collection.Find(c => c.Descricao == convenio).FirstOrDefault();
                            if (convenioCadastrado == null)
                            {
                                convenioCadastrado = new Convenio { Descricao = convenio };
                                convenioCadastrado = convenioService.SaveOne(convenioCadastrado);
                            }
                            agendamento.ConvenioId = convenioCadastrado.Id;

                            var medicoCadastrado = contextoMedico.Collection.Find(c => c.NomeCompleto == nomeMedico).FirstOrDefault();
                            if (medicoCadastrado == null)
                            {
                                medicoCadastrado = new Medico { NomeCompleto = nomeMedico };
                                medicoCadastrado = medicoService.SaveOne(medicoCadastrado);
                            }

                            agendamento.MedicoId = medicoCadastrado.Id;

                            var pacienteCadastrado = contextoPaciente.Collection.Find(c => c.NomeCompleto == nomePaciente).FirstOrDefault();
                            if (pacienteCadastrado == null)
                            {
                                pacienteCadastrado = new Paciente { NomeCompleto = nomePaciente };
                                pacienteCadastrado = pacienteService.SaveOne(pacienteCadastrado);
                            }

                            if (!telefone.IsNullOrWhiteSpace())
                            {
                                var telefoneSplit = telefone.Split("|");
                                pacienteCadastrado.Celular = telefoneSplit[0].RemoverAcentosECaracteresEspeciaisPontosETracos().Trim();
                                
                                if (telefoneSplit.Count() > 1)
                                    pacienteCadastrado.Telefone = telefoneSplit[1].RemoverAcentosECaracteresEspeciaisPontosETracos().Trim();

                                pacienteService.SaveOne(pacienteCadastrado);                                
                            }

                            agendamento.PacienteId = pacienteCadastrado.Id;
                            agendamento.ClinicaId = contextoClinica.Collection.AsQueryable().First().Id;

                            agendamentoService.SaveOne(agendamento);
                        }

                    }

                }
                catch (Exception ex)
                {

                }


            }
        }

        private void AlimentaTabelaFormaPagamento()
        {
            var formaService = new FormaDePagamentoService();
            var formasCadastradas = formaService.GetAll().ToList() ;

            var csvForma = Resource.formaDePagamento;
            using (var reader = new StringReader(csvForma))
            {
                var formas = new Collection<FormaDePagamento>();

                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    var registroForma = line.Split(';');
                    //DESCRIÇÃO;TIPO;DIAS;
                    if (!registroForma[0].Trim().IsNullOrWhiteSpace() && registroForma[1].ToIntOrNull() != null && !formasCadastradas.Any(c=> c.Descricao == registroForma[0].Trim()))
                        formas.Add(new FormaDePagamento
                        {
                            Descricao = registroForma[0].Trim(),
                            TipoPagamento = (EVistaPrazo)registroForma[1].ToInt(),
                            DiasRecebimento = registroForma[2].ToInt()
                        });
                }

                formaService.SalvarDadosFixos(formas);
            }
        }
        private void AlimentaTabelaEspecialidade()
        {
            var especialidadeService = new EspecialidadeService();

            if (especialidadeService.GetAll().HasItems())
                return;

            var csvEspecialidades = Resource.especialidades;
            using (var reader = new StringReader(csvEspecialidades))
            {
                var especialidades = new Collection<Especialidade>();

                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    var registroEspecialidade = line.Split(';');
                    if (!registroEspecialidade[0].Trim().IsNullOrWhiteSpace())
                        especialidades.Add(new Especialidade
                        {
                            Descricao = registroEspecialidade[0].Trim()
                        });
                }

                especialidadeService.SalvarDadosFixos(especialidades);
            }
        }
    }
}
