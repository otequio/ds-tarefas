using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public class TarefaValidator : AbstractValidator<Tarefa>
    {
        public TarefaValidator() 
        { 
            RuleFor(r => r.Titulo)
                .NotEmpty().WithMessage("O título da tarefa não pode ser vazio.")
                .MaximumLength(100).WithMessage("O título da tarefa não pode ter mais de 100 caracteres.");

            RuleFor(r => r.Descricao)
                .MaximumLength(400).WithMessage("A descrição da tarefa não pode ter mais de 400 caracteres.");

            RuleFor(r => r.DataConclusao)
                .GreaterThanOrEqualTo(r => r.DataCriacao)
                .When(r => r.DataConclusao.HasValue)
                .WithMessage("A data de conclusão da tarefa não pode ser menor que a data de criação");

            RuleFor(r => r.DataConclusao)
                .Null()
                .When(t => t.Status != EnumStatus.Concluida)
                .WithMessage("A data de conclusão da tarefa não deve ser informada quando a tarefa não estiver concluída.");

            RuleFor(r => r.DataConclusao)
                .NotNull()
                .When(t => t.Status == EnumStatus.Concluida)
                .WithMessage("A data de conclusão é obrigatória quando a tarefa for concluída");
        }
    }
}
