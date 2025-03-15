using Application.Models;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ITarefaService
    {
        Task<TarefaResponse?> ObterTarefaPorId(int id);
        Task<IEnumerable<TarefaResponse>> ObterTarefasAsync();
        Task<IEnumerable<TarefaResponse>> BuscarTarefasAsync(Expression<Func<Tarefa, bool>> filtro);
        Task<TarefaResponse> CriarTarefaAsync(CriarTarefaRequest tarefaRequest);
        Task AtualizarTarefaAsync(AtualizarTarefaRequest tarefaRequest);
        Task ApagarTarefaAsync(int id);
    }
}
