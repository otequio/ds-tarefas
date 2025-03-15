using System.ComponentModel;

namespace Domain.Enums
{
    public enum EnumStatus
    {
        [Description("Pendente")]
        Pendente = 0,
        [Description("Em Progresso")]
        EmProgresso = 1,
        [Description("Concluída")]
        Concluida = 2
    }
}
