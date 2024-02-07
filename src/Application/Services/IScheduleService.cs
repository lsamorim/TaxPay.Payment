using Application.Services.Dtos;

namespace Application.Services
{
    public interface IScheduleService
    {
        Task<ScheduleServiceOutput> Execute(DateTimeOffset date, CancellationToken cancellationToken);
    }
}
