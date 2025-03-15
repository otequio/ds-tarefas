using Domain.Enums;

namespace Domain.Entities
{
    public class Tarefa : BaseEntity
    {
        public string Titulo { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public DateTime? DataConclusao { get; set; }
        public EnumStatus Status { get; set; } = EnumStatus.Pendente;

        public Tarefa(string titulo, string? descricao)
        {
            Titulo = titulo;
            Descricao = descricao;
        }
    }
}
