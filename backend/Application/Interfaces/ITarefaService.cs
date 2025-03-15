using Application.Models;
using Domain.Entities;
using System.Linq.Expressions;

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
