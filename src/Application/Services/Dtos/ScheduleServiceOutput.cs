using Domain.ScheduleAggregate;

namespace Application.Services.Dtos
{
    public class ScheduleServiceOutput
    {
        public IEnumerable<Schedule> Schedules { get; private set; } = Enumerable.Empty<Schedule>();

        public ScheduleServiceOutput(IEnumerable<Schedule> schedules)
        {
            Schedules = schedules;
        }
    }
}
