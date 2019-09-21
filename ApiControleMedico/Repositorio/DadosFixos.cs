using System.Collections.ObjectModel;
using System.IO;
using ApiControleMedico.Modelos;
using ApiControleMedico.Modelos.Enums;
using ApiControleMedico.Services;
using ApiControleMedico.Uteis;

namespace ApiControleMedico.Repositorio
{
    public class DadosFixos
    {
        public void AlimentarTabelas()
        {

            AlimentaTabelaEspecialidade();
            AlimentaTabelaFormaPagamento();
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
