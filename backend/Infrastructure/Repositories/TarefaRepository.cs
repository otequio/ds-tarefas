using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Repositories
{
    public class TarefaRepository : BaseRepository<Tarefa>, ITarefaRepository
    {
        public TarefaRepository(AppDBContext dbContext) : base(dbContext)
        {
            
        }
    }
}
