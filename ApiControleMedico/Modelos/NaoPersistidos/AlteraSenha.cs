

namespace ApiControleMedico.Modelos.NaoPersistidos
{
    public class AlteraSenha
    {
        public string SenhaAtual { get; set; }
        public string NovaSenha { get; set; }
        public string ConfirmacaoNovaSenha { get; set; }
        public string UsuarioId{ get; set; }

    }
}