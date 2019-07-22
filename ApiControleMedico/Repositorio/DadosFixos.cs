using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ApiControleMedico.Modelos;
using ApiControleMedico.Services;
using ApiControleMedico.Uteis;

namespace ApiControleMedico.Repositorio
{
    public class DadosFixos
    {
        public void AlimentarTabelas()
        {

            AlimentaTabelaEspecialidade();
            
        }

        private void AlimentaTabelaEspecialidade()
        {
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

                new EspecialidadeService().SaveMany(especialidades);
            }
        }
    }
}
 