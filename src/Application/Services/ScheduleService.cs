using Application.Gateways;
using Application.Services.Dtos;
using Domain.ScheduleAggregate;

namespace Application.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly IBank _bank;
        private readonly IGovernment _government;
        private readonly IScheduleRepository _scheduleRepository;

        public ScheduleService(
            IBank bank,
            IGovernment government,
            IScheduleRepository scheduleRepository)
        {
            _bank = bank;
            _government = government;
            _scheduleRepository = scheduleRepository;
        }

        public async Task<ScheduleServiceOutput> Execute(DateOnly date, CancellationToken cancellationToken)
        {
            var schedules = new List<Schedule>();

            // TODO: retrieve the schedules for the date
            // TODO: for each schedule, withdraw from IBank and deposit into IGovernment
            // TODO: add some status control

            return new ScheduleServiceOutput(schedules);
        }
    }
}
