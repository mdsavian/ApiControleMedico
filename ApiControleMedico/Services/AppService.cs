using System.Collections.Generic;
using ApiControleMedico.Modelos;
using ApiControleMedico.Modelos.Enums;
using ApiControleMedico.Uteis;

namespace ApiControleMedico.Services
{
    public class AppService
    {

        public AppService()
        {
        }

        public List<Clinica> BuscarClinicasUsuario(string usuarioid)
        {
            var clinicas = new List<Clinica>();
            List<string> clinicasId;
            var clinicaService = new ClinicaService();
            var usuario = new UsuarioService().GetOne(usuarioid);
            
            if (usuario.TipoUsuario == ETipoUsuario.Comum)
            {
                var funcionario = new FuncionarioService().GetOne(usuario.FuncionarioId);
                clinicasId = funcionario.ClinicasId;
            }
            else
            {
                var medico = new MedicoService().GetOne(usuario.MedicoId);
                clinicasId = medico.ClinicasId;
            }
            if (clinicasId != null && clinicasId.HasItems())
            {
                foreach (var idClinica in clinicasId)
                {
                    clinicas.Add(clinicaService.GetOne(idClinica));
                }
            }

            return clinicas;

        }


    }
}