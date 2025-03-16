using Domain.Enums;
using Infrastructure.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.Models
{
    public record AtualizarTarefaRequest
    (
        [Required(ErrorMessage = "O Id é obrigatório")]
        int Id,
        [Required(ErrorMessage = "O título é obrigatório")]
        [MaxLength(100, ErrorMessage = "O título pode ter no máximo 100 caracteres")]
        string Titulo,
        [MaxLength(400, ErrorMessage = $"A descrição pode ter no máximo 400 caracteres")]
        string? Descricao,
        [property: JsonConverter(typeof(NullableDateTimeConverter))]
        DateTime? DataConclusao,
        [property: JsonConverter(typeof(JsonStringEnumConverter))]
        EnumStatus Status  = EnumStatus.Pendente
    );
}
