using Domain.Seedwork;

namespace Domain.ScheduleAggregate
{
    public interface IScheduleRepository : IRepository
    {
        Task<IEnumerable<Schedule>> GetSchedulesFor(DateTimeOffset date, CancellationToken cancellationToken);
    }
}
