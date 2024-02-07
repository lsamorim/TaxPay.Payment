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

        public async Task<ScheduleServiceOutput> Execute(DateTimeOffset date, CancellationToken cancellationToken)
        {
            var schedules = await _scheduleRepository.GetSchedulesFor(date, cancellationToken);
            foreach (var schedule in schedules)
            {
                await Settle(schedule, cancellationToken);
            }

            return new ScheduleServiceOutput(schedules);
        }

        private async Task Settle(Schedule schedule, CancellationToken cancellationToken)
        {
            try
            {
                var withdrawOutput = await _bank.Withdraw(schedule.BankAccount, schedule.Amount, cancellationToken);
                
                if (!withdrawOutput.Success)
                {
                    schedule.Failed(withdrawOutput.Message);
                    return;
                }
                
                var depositOutput = await _government.Deposit(schedule.BankAccount, schedule.Amount, cancellationToken);
                if (!depositOutput.Success)
                {
                    schedule.Failed(depositOutput.Message);
                    var refundOutput = await _bank.Deposit(schedule.BankAccount, schedule.Amount, cancellationToken);
                    
                    if (!refundOutput.Success)
                    {
                        schedule.Failed(refundOutput.Message);
                        return;
                    }

                    return;
                }

                schedule.Completed();
            }
            catch (Exception)
            {
                schedule.Failed("An exception has ocurrred");
            }
        }
    }
}
