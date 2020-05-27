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
            //AlimentarPacientes();
        }

        private void AlimentarPacientes()
        {
            var contextoPaciente = new DbContexto<Paciente>("paciente");
            var pacienteService = new PacienteService();

            //var csvPacientes = Resource.pacientes;
            //using (var reader = new StringReader(csvPacientes))
            //{
            //    string line;
            //    try
            //    {
            //        while ((line = reader.ReadLine()) != null)
            //        {
            //            //Nome Completo;CPF;Telefone;Celular;E-mail;Nascimento;Ocupação;Data cadastro
            //            var linhaAgendamento = line.Split(';');

            //            if (!linhaAgendamento[0].Trim().IsNullOrWhiteSpace() && linhaAgendamento[0].Trim() != "Nome Completo")
            //            {
            //                var nomePaciente = linhaAgendamento[0].ToUpper();
            //                var paciente = contextoPaciente.Collection.AsQueryable()
            //                    .FirstOrDefault(c => c.NomeCompleto == nomePaciente);

            //                if (paciente != null)
            //                    continue;

            //                paciente = new Paciente
            //                {
            //                    NomeCompleto = linhaAgendamento[0],
            //                    CpfCnpj = linhaAgendamento[1],
            //                    Telefone = linhaAgendamento[2].RemoverAcentosECaracteresEspeciaisPontosETracos().Replace(" ", string.Empty).Trim(),
            //                    Celular = linhaAgendamento[3].RemoverAcentosECaracteresEspeciaisPontosETracos().Replace(" ", string.Empty).Trim(),
            //                    Email = linhaAgendamento[4],
            //                    DataNascimento = linhaAgendamento[5].ToDateTimeOrNull(),
            //                    Ocupacao = linhaAgendamento[6],
            //                    DataCadastro = linhaAgendamento[0].ToDateTimeOrNull()
            //                };

            //                pacienteService.SaveOne(paciente);

            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {

            //    }
            //}
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
                        //Médico;Paciente;Data Agendamento;Tipo;Descricao;Convênio;Primeiro;OBS;Hora.
                        var linhaAgendamento = line.Split(';');

                        if (!linhaAgendamento[0].Trim().IsNullOrWhiteSpace() && linhaAgendamento[2].ToDateTimeOrNull() != null)
                        {
                            var agendamento = new Agendamento();

                            var nomeMedico = linhaAgendamento[0].ToUpper();
                            var nomePaciente = linhaAgendamento[1].ToUpper();
                            var dataAgendamento = linhaAgendamento[2].ToDateTime();
                            var tipoAgendamento = linhaAgendamento[3];
                            var descricao = linhaAgendamento[4].ToUpper();
                            var convenio = linhaAgendamento[5].ToUpper();
                            var primeiroAtendimento = linhaAgendamento[6];
                            var horaAtendimento = linhaAgendamento[8].Trim().Replace(":", "").Substring(0,4);
                            var EnumTipoAgendamento = (ETipoAgendamento)Enum.Parse(typeof(ETipoAgendamento), tipoAgendamento);

                            var data = new DateTime();
                            data = data.AddHours(horaAtendimento.Substring(0, 2).ToInt()).AddMinutes(horaAtendimento.Substring(2, 2).ToInt());

                            agendamento.DataAgendamento = dataAgendamento.Date;
                            agendamento.Observacao = linhaAgendamento[7];
                            agendamento.HoraInicial = horaAtendimento;
                            agendamento.HoraFinal = data.AddMinutes(20).FormatarHora24().Replace(":", "");
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
            var formasCadastradas = formaService.GetAll().ToList();

            var csvForma = Resource.formaDePagamento;
            using (var reader = new StringReader(csvForma))
            {
                var formas = new Collection<FormaDePagamento>();

                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    var registroForma = line.Split(';');
                    //DESCRIÇÃO;TIPO;DIAS;
                    if (!registroForma[0].Trim().IsNullOrWhiteSpace() && registroForma[1].ToIntOrNull() != null && !formasCadastradas.Any(c => c.Descricao == registroForma[0].Trim()))
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
