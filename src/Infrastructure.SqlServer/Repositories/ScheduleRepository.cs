using Domain.ScheduleAggregate;
using Infrastructure.SqlServer.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SqlServer.Repositories
{
    public class ScheduleRepository : BaseRepository<PaymentContext>, IScheduleRepository
    {
        public ScheduleRepository(PaymentContext context) : base(context) { }

        public async Task<IEnumerable<Schedule>> GetSchedulesFor(DateTimeOffset date, CancellationToken cancellationToken)
        {
            return await _context.Schedules
                .AsTracking()
                .Where(schedule => schedule.ScheduleDate == date)
                .ToListAsync(cancellationToken);
        }
    }
}
