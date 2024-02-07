using Domain.Seedwork;
using Infrastructure.SqlServer.Common;

namespace Infrastructure.SqlServer.Repositories
{
    public class BaseRepository<TContext> : IRepository
        where TContext : BaseDbContext
    {
        public IUnitOfWork UnitOfWork => _context;

        protected TContext _context;

        public BaseRepository(TContext context)
        {
            _context = context;
        }
    }
}
