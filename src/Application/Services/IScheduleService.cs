using Application.Services.Dtos;

namespace Application.Services
{
    public interface IScheduleService
    {
        Task<ScheduleServiceOutput> Execute(DateOnly date, CancellationToken cancellationToken);
    }
}
