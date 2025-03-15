using Domain.Enums;
using System.Text.Json.Serialization;

namespace Application.Models
{
    public record TarefaResponse
    (
        int Id,
        string Titulo,
        string? Descricao,
        DateTime DataCriacao,
        DateTime? DataConclusao,
        [property: JsonConverter(typeof(JsonStringEnumConverter))]
        EnumStatus Status
    );
}
