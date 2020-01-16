using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
            //AlimentaAgendamentos();
        }

        private void AlimentaAgendamentos()
        {
            var agendamentoService = new AgendamentoService();
            var medicoService = new MedicoService();
            var pacienteService = new PacienteService();
            var convenioService = new ConvenioService();
            var cirurgiaService = new CirurgiaService();
            var procedimentoService = new ProcedimentoService();

            var contextoMedico = new DbContexto<Medico>("medico");
            var contextoPaciente = new DbContexto<Paciente>("paciente");
            var contextoConvenio = new DbContexto<Convenio>("convenio");
            var contextoCirurgia = new DbContexto<Cirurgia>("cirurgia");
            var contextoProcedimento = new DbContexto<Procedimento>("procedimento");

            var csvAgendamento = Resource.agendas_novembro_importacao;
            using (var reader = new StringReader(csvAgendamento))
            {
                string line;
                try
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        //Médico;Paciente;Data Agendamento;Tipo;Descricao;Convênio;Status;OBS;
                        var linhaAgendamento = line.Split(';');

                        if (!linhaAgendamento[0].Trim().IsNullOrWhiteSpace() && linhaAgendamento[2].ToDateTimeOrNull() != null)
                        {
                            var agendamento = new Agendamento();                            

                            var nomeMedico = linhaAgendamento[0];
                            var nomePaciente = linhaAgendamento[1];
                            var dataAgendamento = linhaAgendamento[2].ToDateTime();
                            var tipoAgendamento = linhaAgendamento[3];
                            var descricao = linhaAgendamento[4];
                            var convenio = linhaAgendamento[5];
                            var statusAgendamento = linhaAgendamento[6];
                            var EnumTipoAgendamento = (ETipoAgendamento)Enum.Parse(typeof(ETipoAgendamento), tipoAgendamento);
                            
                            agendamento.Observacao = linhaAgendamento[7];
                            agendamento.DataAgendamento = dataAgendamento.Date;
                            agendamento.HoraInicial = dataAgendamento.FormatarHora24().Replace(":", "");
                            agendamento.HoraFinal = dataAgendamento.AddMinutes(20).FormatarHora24().Replace(":", "");

                            switch (statusAgendamento)
                            {
                                case "Finalizado":
                                    {
                                        agendamento.SituacaoAgendamento = ESituacaoAgendamento.Finalizado;
                                        break;
                                    }
                                case "Em espera":
                                case "NAO confirmado":
                                    {
                                        agendamento.SituacaoAgendamento = ESituacaoAgendamento.Agendado;
                                        break;
                                    }
                                default:
                                    {
                                        agendamento.SituacaoAgendamento = (ESituacaoAgendamento)Enum.Parse(typeof(ESituacaoAgendamento), statusAgendamento);
                                        break;
                                    }
                            }

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
                            agendamento.PacienteId = pacienteCadastrado.Id;

                            if(agendamento.SituacaoAgendamento == ESituacaoAgendamento.Finalizado)
                            {
                                agendamento.Pagamentos = new List<AgendamentoPagamento>();

                                Random random = new Random();
                                int randomNumber = random.Next(0, 1000);
                                
                                agendamento.Pagamentos.Add(new AgendamentoPagamento
                                {
                                    Valor = new Random().Next(20, 200),
                                    FormaPagamentoId = "5d6c544f27d733093815c335",
                                    Data = agendamento.DataAgendamento,
                                    Parcela = 1,
                                    VistaPrazo = EVistaPrazo.Vista
                                });
                            }

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

            if (formaService.GetAll().HasItems())
                return;

            var csvForma = Resource.formaDePagamento;
            using (var reader = new StringReader(csvForma))
            {
                var formas = new Collection<FormaDePagamento>();

                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    var registroForma = line.Split(';');
                    //DESCRIÇÃO;TIPO;DIAS;
                    if (!registroForma[0].Trim().IsNullOrWhiteSpace() && registroForma[1].ToIntOrNull() != null)
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
