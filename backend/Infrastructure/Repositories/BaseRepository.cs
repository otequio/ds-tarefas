using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class BaseRepository<T>(DbContext _context) : IBaseRepository<T> where T : BaseEntity
    {
        public async Task InserirAsync(T obj)
        {
            await _context.Set<T>().AddAsync(obj);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(T obj)
        {
            _context.Set<T>().Attach(obj);
            _context.Entry(obj).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeletarAsync(T obj)
        {
            _context.Set<T>().Remove(obj);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> BuscarAsync(Expression<Func<T, bool>> filtro)
        {
            return await _context
                .Set<T>()
                .Where(filtro)
                .OrderByDescending(r => r.Id)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> ObterTodosAsync()
        {
            return await _context
                .Set<T>()
                .OrderByDescending(r => r.Id)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<T?> ObterPorIdAsync(int id)
        {
            return await _context
                .Set<T>()
                .FindAsync(id);
        }
    }
}
