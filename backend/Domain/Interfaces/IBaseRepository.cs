using Domain.Entities;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Domain.Interfaces
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task InserirAsync(T obj);
        Task AtualizarAsync(T obj);
        Task DeletarAsync(T obj);
        Task<T?> ObterPorIdAsync(int id);
        Task<IEnumerable<T>> ObterTodosAsync();
        Task<IEnumerable<T>> BuscarAsync(Expression<Func<T, bool>> filtro);

    }
}
