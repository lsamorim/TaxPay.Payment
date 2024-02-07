using Domain.ScheduleAggregate;
using Infrastructure.SqlServer.Common;
using Infrastructure.SqlServer.Configurations.ScheduleAggregate;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SqlServer.Context
{
    public class PaymentContext : BaseDbContext
    {
        public PaymentContext()
        {
            
        }

        public PaymentContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Schedule> Schedules => Set<Schedule>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ScheduleConfiguration());
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PaymentContext).Assembly);
        }
    }
}
