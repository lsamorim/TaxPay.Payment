namespace Domain.ScheduleAggregate
{
    public interface IScheduleRepository
    {
        Task<IEnumerable<Schedule>> GetSchedulesFor(DateOnly date, CancellationToken cancellationToken);
    }
}
