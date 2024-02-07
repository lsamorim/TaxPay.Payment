using Domain.Seedwork;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SqlServer.Common
{
    public abstract class BaseDbContext : DbContext, IUnitOfWork
    {
        public BaseDbContext()
        {
            
        }

        public BaseDbContext(DbContextOptions options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
        }

        public async Task<int> SaveChanges(CancellationToken cancellationToken)
        {
            var rowsAffected = await base.SaveChangesAsync(cancellationToken);
            return rowsAffected;
        }
    }
}
