using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public record CriarTarefaRequest
    (
        [Required(ErrorMessage = "O título é obrigatório")]
        [MaxLength(100, ErrorMessage = $"O título pode ter no máximo 100 caracteres")]
        string Titulo,
        [MaxLength(400, ErrorMessage = $"A descrição pode ter no máximo 100 caracteres")]
        string? Descricao 
    );
}
