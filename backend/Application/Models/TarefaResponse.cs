using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
